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

// if on >= NETStandard1.3 you can use:
// IgesFile igesFile = IgesFile.Load(@"C:\Path\To\File.iges");

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

using (FileStream fs = new FileStream(@"C:\Path\To\File.iges", FileMode.Create))
{
    igesFile.Save(fs);
}

// if on >= .NETStandard1.3 you can use:
// igesFile.Save(@"C:\Path\To\File.iges");
```

## Building locally

To build locally, install the [latest .NET Core 3.0 SDK](https://dotnet.microsoft.com/download).

## IGES reference

[Full specification (from uspro.org)](http://www.uspro.org/documents/IGES5-3_forDownload.pdf)

## Sample files

Sample files can be found [here](http://www.wiz-worx.com/iges5x/).  Of particular note are the following categories:

- [Class 1](http://www.wiz-worx.com/iges5x/onetwo/class1.shtml)

- [Class 2](http://www.wiz-worx.com/iges5x/onetwo/class2.shtml)

- [Class 7](http://www.wiz-worx.com/iges5x/onetwo/class7.shtml)
