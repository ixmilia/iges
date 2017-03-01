IxMilia.Iges
============

A portable .NET library for reading and writing IGES files.

## Usage

Open an IGES file:

``` C#
using System.IO;
using IxMilia.Iges;
using IxMilia.Iges.Entities;
// ...

IgesFile igesFile;
using (FileStream fs = new FileStream(@"C:\Path\To\File.iges", FileMode.Open))
{
    igesFile = IgesFile.Load(fs);
}

foreach (IgesEntity entity in igesFile.Entities)
{
    switch (entity.EntityType)
    {
        case IgesEntityType.Line:
            IgesLine line = (IgesLine)entity;
            // ...
            break;
        // ...
    }
}
```

Save a IGES file:

``` C#
using System.IO;
using IxMilia.Iges;
using IxMilia.Iges.Entities;
// ...

IgesFile igesFile = new IgesFile();
igesFile.Entities.Add(new IgesLine() { P1 = new IgesPoint(0, 0, 0), P2 = new IgesPoint(50, 50, 0) });
// ...

using (FileStream fs = new FileStream(@"C:\Path\To\File.iges", FileMode.Open))
{
    igesFile.Save(fs);
}
```

## Building locally

Requirements to build locally are:

- [Latest .NET Core SDK](https://github.com/dotnet/cli/releases)  As of this writing the following was also required on Ubuntu 14.04:

`sudo apt-get install dotnet-sharedframework-microsoft.netcore.app-1.0.3`


## IGES reference

[Full specification (from uspro.org)](http://www.uspro.org/documents/IGES5-3_forDownload.pdf)

## Sample files

Sample files can be found [here](http://www.wiz-worx.com/iges5x/).  Of particular note are the following categories:

- [Class 1](http://www.wiz-worx.com/iges5x/onetwo/class1.shtml)

- [Class 2](http://www.wiz-worx.com/iges5x/onetwo/class2.shtml)

- [Class 7](http://www.wiz-worx.com/iges5x/onetwo/class7.shtml)
