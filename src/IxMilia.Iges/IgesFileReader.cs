using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using IxMilia.Iges.Entities;

namespace IxMilia.Iges
{
    internal class IgesFileReader
    {
        public static IgesFile Load(Stream stream)
        {
            var file = new IgesFile();
            var allLines = new StreamReader(stream).ReadToEnd().Split("\n".ToCharArray()).Select(s => s.TrimEnd()).Where(line => !string.IsNullOrEmpty(line));
            string terminateLine = null;
            var startLines = new List<string>();
            var globalLines = new List<string>();
            var directoryLines = new List<string>();
            var parameterLines = new List<string>();
            var sectionLines = new Dictionary<IgesSectionType, List<string>>()
                {
                    { IgesSectionType.Start, startLines },
                    { IgesSectionType.Global, globalLines },
                    { IgesSectionType.Directory, directoryLines },
                    { IgesSectionType.Parameter, parameterLines }
                };

            foreach (var line in allLines)
            {
                if (line.Length != 80)
                    throw new IgesException("Expected line length of 80 characters.");
                var data = line.Substring(0, IgesFile.MaxDataLength);
                var sectionType = SectionTypeFromCharacter(line[IgesFile.MaxDataLength]);
                var lineNumber = IgesParser.ParseIntStrict(line.Substring(IgesFile.MaxDataLength + 1).TrimStart());

                if (sectionType == IgesSectionType.Terminate)
                {
                    if (terminateLine != null)
                        throw new IgesException("Unexpected duplicate terminate line");
                    terminateLine = data;

                    // verify terminate data and quit
                    var startCount = IgesParser.ParseIntStrict(terminateLine.Substring(1, 7));
                    var globalCount = IgesParser.ParseIntStrict(terminateLine.Substring(9, 7));
                    var directoryCount = IgesParser.ParseIntStrict(terminateLine.Substring(17, 7));
                    var parameterCount = IgesParser.ParseIntStrict(terminateLine.Substring(25, 7));
                    if (startLines.Count != startCount)
                        throw new IgesException("Incorrect number of start lines reported");
                    if (globalLines.Count != globalCount)
                        throw new IgesException("Incorrect number of global lines reported");
                    if (directoryLines.Count != directoryCount)
                        throw new IgesException("Incorrect number of directory lines reported");
                    if (parameterLines.Count != parameterCount)
                        throw new IgesException("Incorrect number of parameter lines reported");
                    break;
                }
                else
                {
                    if (sectionType == IgesSectionType.Parameter)
                        data = data.Substring(0, data.Length - 8); // parameter data doesn't need its last 8 bytes
                    sectionLines[sectionType].Add(data);
                    if (sectionLines[sectionType].Count != lineNumber)
                        throw new IgesException("Unordered line number");
                }
            }

            // don't worry if terminate line isn't present

            ParseGlobalLines(file, globalLines);
            var directoryEntries = ParseDirectoryLines(directoryLines);
            var parameterMap = PrepareParameterLines(parameterLines, directoryEntries, file.FieldDelimiter, file.RecordDelimiter);
            PopulateEntities(file, directoryEntries, parameterMap);

            return file;
        }

        private enum ParameterParseState
        {
            ParsingEntityNumber,
            ParsingNumber,
            ParsingString,
            ParsingComment
        }

        private static List<IgesDirectoryData> ParseDirectoryLines(List<string> directoryLines)
        {
            var directoryEntires = new List<IgesDirectoryData>();
            for (int i = 0; i < directoryLines.Count; i += 2)
            {
                var dir = IgesDirectoryData.FromRawLines(directoryLines[i], directoryLines[i + 1]);
                directoryEntires.Add(dir);
            }

            return directoryEntires;
        }

        private static Dictionary<int, Tuple<List<string>, string>> PrepareParameterLines(List<string> parameterLines, IEnumerable<IgesDirectoryData> directoryEntires, char fieldDelimiter, char recordDelimiter)
        {
            var map = new Dictionary<int, Tuple<List<string>, string>>();
            var fields = new List<string>();
            var state = ParameterParseState.ParsingEntityNumber;
            var current = new StringBuilder();
            int entityNumber = 0;
            int stringLength = 0;

            foreach (var dir in directoryEntires)
            {
                var lowerBound = dir.ParameterPointer - 1;
                var upperBound = lowerBound + dir.LineCount;
                for (int i = lowerBound; i < upperBound; i++)
                {
                    var line = parameterLines[i];

                    // for each character
                    for (int j = 0; j < line.Length; j++)
                    {
                        var ch = line[j];
                        if (state == ParameterParseState.ParsingString)
                        {
                            if (current.Length == stringLength)
                            {
                                fields.Add(current.ToString());
                                current.Clear();
                                if (ch == fieldDelimiter)
                                {
                                    state = ParameterParseState.ParsingNumber;
                                }
                                else if (ch == recordDelimiter)
                                {
                                    state = ParameterParseState.ParsingComment;
                                }
                                else
                                {
                                    Debug.Assert(false, "expected field or record delimiter");
                                }
                            }
                            else
                            {
                                current.Append(ch);
                            }
                        }
                        else if (state == ParameterParseState.ParsingComment)
                        {
                            current.Append(ch);
                        }
                        else
                        {
                            if (ch == fieldDelimiter)
                            {
                                if (state == ParameterParseState.ParsingEntityNumber)
                                {
                                    entityNumber = IgesParser.ParseIntStrict(current.ToString());
                                }
                                else
                                {
                                    fields.Add(current.ToString());
                                }

                                state = ParameterParseState.ParsingNumber;
                                current.Clear();
                            }
                            else if (ch == recordDelimiter)
                            {
                                if (state == ParameterParseState.ParsingEntityNumber)
                                {
                                    entityNumber = IgesParser.ParseIntStrict(current.ToString());
                                }
                                else
                                {
                                    fields.Add(current.ToString());
                                }

                                state = ParameterParseState.ParsingComment;
                                current.Clear();
                            }
                            else if (ch == IgesFile.StringSentinelCharacter)
                            {
                                stringLength = IgesParser.ParseIntStrict(current.ToString());
                                current.Clear();
                                state = ParameterParseState.ParsingString;
                            }
                            else
                            {
                                current.Append(ch);
                            }
                        }
                    }
                }

                var comment = current.ToString().Trim();

                // clean up escaped comemnt values
                comment = comment.Replace("\\\\", "\\");
                comment = comment.Replace("\\n", "\n");
                comment = comment.Replace("\\r", "\r");
                comment = comment.Replace("\\t", "\t");
                comment = comment.Replace("\\v", "\v");
                comment = comment.Replace("\\f", "\f");

                current.Clear();
                map[dir.ParameterPointer] = Tuple.Create(fields, string.IsNullOrEmpty(comment) ? null : comment);
                fields = new List<string>();
                state = ParameterParseState.ParsingEntityNumber;
            }

            return map;
        }

        private static void PopulateEntities(IgesFile file, List<IgesDirectoryData> directoryEntries, Dictionary<int, Tuple<List<string>, string>> parameterMap)
        {
            var binder = new IgesReaderBinder();
            for (int i = 0; i < directoryEntries.Count; i++)
            {
                var dir = directoryEntries[i];
                var parameterValues = parameterMap[dir.ParameterPointer].Item1;
                var comment = parameterMap[dir.ParameterPointer].Item2;
                var entity = IgesEntity.FromData(dir, parameterValues, binder);
                if (entity != null)
                {
                    entity.Comment = comment;
                    var directoryIndex = (i * 2) + 1;
                    entity.BindPointers(dir, binder);
                    entity.OnAfterRead(dir);
                    var postProcessed = entity.PostProcess();
                    binder.EntityMap[directoryIndex] = postProcessed;
                    file.Entities.Add(postProcessed);
                }
            }

            binder.BindRemainingEntities();
        }

        private static void ParseGlobalLines(IgesFile file, List<string> globalLines)
        {
            var fullString = string.Join(string.Empty, globalLines).TrimEnd();
            if (string.IsNullOrEmpty(fullString))
                return;

            int index = 0;
            ParseDelimiterCharacter(file, fullString, ref index, true); // 1
            ParseDelimiterCharacter(file, fullString, ref index, false); // 2
            file.Identification = ParseString(file, fullString, ref index); // 3
            file.FullFileName = ParseString(file, fullString, ref index); // 4
            file.SystemIdentifier = ParseString(file, fullString, ref index); // 5
            file.SystemVersion = ParseString(file, fullString, ref index); // 6
            file.IntegerSize = ParseInt(file, fullString, ref index); // 7
            file.SingleSize = ParseInt(file, fullString, ref index); // 8
            file.DecimalDigits = ParseInt(file, fullString, ref index); // 9
            file.DoubleMagnitude = ParseInt(file, fullString, ref index); // 10
            file.DoublePrecision = ParseInt(file, fullString, ref index); // 11
            file.Identifier = ParseString(file, fullString, ref index); // 12
            file.ModelSpaceScale = ParseDouble(file, fullString, ref index); // 13
            file.ModelUnits = (IgesUnits)ParseInt(file, fullString, ref index, (int)file.ModelUnits); // 14
            file.CustomModelUnits = ParseString(file, fullString, ref index); // 15
            file.MaxLineWeightGraduations = ParseInt(file, fullString, ref index); // 16
            file.MaxLineWeight = ParseDouble(file, fullString, ref index); // 17
            file.TimeStamp = ParseDateTime(ParseString(file, fullString, ref index), file.TimeStamp); // 18
            file.MinimumResolution = ParseDouble(file, fullString, ref index); // 19
            file.MaxCoordinateValue = ParseDouble(file, fullString, ref index); // 20
            file.Author = ParseString(file, fullString, ref index); // 21
            file.Organization = ParseString(file, fullString, ref index); // 22
            file.IgesVersion = (IgesVersion)ParseInt(file, fullString, ref index); // 23
            file.DraftingStandard = (IgesDraftingStandard)ParseInt(file, fullString, ref index); // 24
            file.ModifiedTime = ParseDateTime(ParseString(file, fullString, ref index), file.ModifiedTime); // 25
            file.ApplicationProtocol = ParseString(file, fullString, ref index); // 26
        }

        private static void ParseDelimiterCharacter(IgesFile file, string str, ref int index, bool readFieldSeparator)
        {
            // verify length
            if (index >= str.Length)
                throw new IgesException("Unexpected end of input");

            // could be empty
            if (readFieldSeparator)
            {
                if (str[index] == IgesFile.DefaultFieldDelimiter)
                {
                    index++;
                    return;
                }
            }
            else
            {
                if (str[index] == file.FieldDelimiter || str[index] == IgesFile.DefaultRecordDelimiter)
                {
                    index++;
                    return;
                }
            }

            if (str[index] != '1')
                throw new IgesException("Expected delimiter of length 1");
            index++;

            // verify 'H' separator
            if (index >= str.Length)
                throw new IgesException("Unexpected end of input");
            if (str[index] != IgesFile.StringSentinelCharacter)
                throw new IgesException("Unexpected string sentinel character");
            index++;

            // get the separator character and set it
            if (index >= str.Length)
                throw new IgesException("Expected delimiter character");
            var separator = str[index];
            if (readFieldSeparator)
            {
                file.FieldDelimiter = separator;
            }
            else
            {
                if (separator == file.FieldDelimiter)
                {
                    throw new IgesException("Record delimiter cannot match field delimiter");
                }

                file.RecordDelimiter = separator;
            }
            index++;

            // verify delimiter
            if (index >= str.Length)
                throw new IgesException("Unexpected end of input");
            separator = str[index];
            if (separator != file.FieldDelimiter && separator != file.RecordDelimiter)
                throw new IgesException("Expected field or record delimiter");
            index++; // swallow it
        }

        private static string ParseString(IgesFile file, string str, ref int index, string defaultValue = null)
        {
            if (index < str.Length && (str[index] == file.FieldDelimiter || str[index] == file.RecordDelimiter))
            {
                // swallow the delimiter and return the default
                index++;
                return defaultValue;
            }

            SwallowWhitespace(str, ref index);

            var sb = new StringBuilder();

            // parse length
            for (; index < str.Length; index++)
            {
                var c = str[index];
                if (c == IgesFile.StringSentinelCharacter)
                {
                    index++; // swallow H
                    break;
                }
                if (!char.IsDigit(c))
                    throw new IgesException("Expected digit");
                sb.Append(c);
            }

            var lengthString = sb.ToString();
            if (string.IsNullOrWhiteSpace(lengthString))
            {
                return defaultValue;
            }

            int length = IgesParser.ParseIntStrict(lengthString);
            sb.Clear();

            // parse content
            var value = str.Substring(index, length);
            index += length;

            // verify delimiter and swallow
            if (index == str.Length - 1)
                SwallowDelimiter(str, file.RecordDelimiter, ref index);
            else
                SwallowDelimiter(str, file.FieldDelimiter, ref index);

            return value;
        }

        private static int ParseInt(IgesFile file, string str, ref int index, int defaultValue = 0)
        {
            if (index < str.Length && (str[index] == file.FieldDelimiter || str[index] == file.RecordDelimiter))
            {
                // swallow the delimiter and return the default
                index++;
                return defaultValue;
            }

            SwallowWhitespace(str, ref index);

            var sb = new StringBuilder();
            for (; index < str.Length; index++)
            {
                var c = str[index];
                if (c == file.FieldDelimiter || c == file.RecordDelimiter)
                {
                    index++; // swallow it
                    break;
                }
                if (!char.IsDigit(c))
                    throw new IgesException("Expected digit");
                sb.Append(c);
            }

            var intString = sb.ToString();
            if (string.IsNullOrWhiteSpace(intString))
                return defaultValue;
            else
                return IgesParser.ParseIntStrict(sb.ToString());
        }

        private static double ParseDouble(IgesFile file, string str, ref int index, double defaultValue = 0.0)
        {
            if (index < str.Length && (str[index] == file.FieldDelimiter || str[index] == file.RecordDelimiter))
            {
                // swallow the delimiter and return the default
                index++;
                return defaultValue;
            }

            SwallowWhitespace(str, ref index);

            var sb = new StringBuilder();
            for (; index < str.Length; index++)
            {
                var c = str[index];
                if (c == file.FieldDelimiter || c == file.RecordDelimiter)
                {
                    index++; // swallow it
                    break;
                }
                sb.Append(c);
            }

            var doubleString = sb.ToString();
            if (string.IsNullOrWhiteSpace(doubleString))
                return defaultValue;
            else
                return IgesParser.ParseDoubleStrict(sb.ToString());
        }

        internal static DateTime ParseDateTime(string value, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return DateTime.Now;
            }

            var match = dateTimeReg.Match(value);
            if (!match.Success)
                throw new IgesException("Invalid date/time format");
            Debug.Assert(match.Groups.Count == 9);
            int year = IgesParser.ParseIntStrict(match.Groups[1].Value);
            int month = IgesParser.ParseIntStrict(match.Groups[4].Value);
            int day = IgesParser.ParseIntStrict(match.Groups[5].Value);
            int hour = IgesParser.ParseIntStrict(match.Groups[6].Value);
            int minute = IgesParser.ParseIntStrict(match.Groups[7].Value);
            int second = IgesParser.ParseIntStrict(match.Groups[8].Value);
            if (match.Groups[1].Value.Length == 2)
                year += 1900;
            return new DateTime(year, month, day, hour, minute, second);
        }

        private static void SwallowWhitespace(string str, ref int index)
        {
            for (; index < str.Length; index++)
            {
                var c = str[index];
                if (!char.IsWhiteSpace(c))
                    break;
            }
        }

        private static Regex dateTimeReg = new Regex(@"((\d{2})|(\d{4}))(\d{2})(\d{2})\.(\d{2})(\d{2})(\d{2})");
        //                                             12       3       4      5        6      7      8

        private static void SwallowDelimiter(string str, char delim, ref int index)
        {
            if (index >= str.Length)
                throw new IgesException("Unexpected end of string");
            if (str[index++] != delim)
                throw new IgesException("Expected delimiter");
        }

        private static IgesSectionType SectionTypeFromCharacter(char c)
        {
            switch (c)
            {
                case 'S': return IgesSectionType.Start;
                case 'G': return IgesSectionType.Global;
                case 'D': return IgesSectionType.Directory;
                case 'P': return IgesSectionType.Parameter;
                case 'T': return IgesSectionType.Terminate;
                default:
                    throw new IgesException("Invalid section type " + c);
            }
        }
    }
}
