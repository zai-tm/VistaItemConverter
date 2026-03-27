# VistaItemConverter
A tool to convert items to use the Vista materials instead of Stadium materials

## Usage
Download and extract the zip in the releases page. Then just drop a folder on the executable and it will create the variants for each vista. The folder must be in your TM items folder.

## What gets converted?
This is the list of materials that the game automatically converts in the vistas:

- Stadium\Media\Material\Deco
- Stadium\Media\Material\DecoHill
- Stadium\Media\Material\DecoHill2
- Stadium\Media\Modifier\PlatformDirt\Deco
- Stadium\Media\Modifier\PlatformDirt\DecoHill
- Stadium\Media\Modifier\PlatformDirt\DecoHill2
- Stadium\Media\Modifier\PlatformIce\Deco
- Stadium\Media\Modifier\PlatformIce\DecoHill
- Stadium\Media\Modifier\PlatformIce\DecoHill2
- Stadium\Media\Material\TrackWallClips
- Stadium\Media\Material\TrackBorders
- Stadium\Media\Material\TrackBordersOff
- Stadium\Media\Material\DecalPaintSponsor4x1D
- Stadium\Media\Material\DecalPaint2Sponsor4x1D
- Stadium\Media\Material\DecalPaintSponsor4x1NoColorizeD
- Stadium\Media\Material\DecalPaint2Sponsor4x1NoColorizeD
- Stadium\Media\Material\TrackWall

## Mesh Modeler items
Unfortunately, the mesh modeler doesn't like "illegal" materials. The program skips mesh modeler items over, since it's impossible to convert them to NadeoImporter (solid2) items. There is a workaround though:

1. Place the mesh modeler items you want to convert in a map and save it
2. Extract the items at https://io.gbx.tools/extract-embedded-items
3. Move the items in the extracted zip into your items folder (create a backup of the originals if you want to be able to edit them later)
4. The game converted the items to Solid2, so they are able to be converted by the tool.
