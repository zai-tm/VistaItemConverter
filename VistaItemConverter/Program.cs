using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.LZO;
using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using static GBX.NET.Engines.Game.CGameCtnChallenge;

if (args.Length < 1)
{
    foreach (var arg in args)
    {
        Console.Write(arg+" ");
    }
    Console.WriteLine();
    Console.WriteLine("Please specify the directory like this:");
    Console.WriteLine("VistaItemConverter.exe C:\\users\\example\\path\\to\\items\\subfolder");
    Console.WriteLine("Or drag the folder containing the items you want to convert onto the executable.");
    Exit();
}
if (!Directory.Exists(args[0]))
{
    Console.WriteLine("Not a valid directory");
    Exit();
}
string[] allFiles = Directory.GetFiles(args[0], "*.Item.Gbx", SearchOption.AllDirectories);
string[] vistas = ["RedIsland", "GreenCoast", "BlueBay", "WhiteShore"];
Dictionary<string, string> vistasMaterialsMapping =
new Dictionary<string, string>()
{
    { @"Stadium\Media\Material\Deco", @"\Media\Modifier\StadiumOnTerrain\Deco" },
    { @"Stadium\Media\Material\DecoHill", @"\Media\Modifier\StadiumOnTerrain\DecoHill" },
    { @"Stadium\Media\Material\DecoHill2", @"\Media\Modifier\StadiumOnTerrain\DecoHill2" },
    { @"Stadium\Media\Modifier\PlatformDirt\Deco", @"\Media\Modifier\StadiumOnTerrain\Deco" },
    { @"Stadium\Media\Modifier\PlatformDirt\DecoHill", @"\Media\Modifier\StadiumOnTerrain\DecoHill" },
    { @"Stadium\Media\Modifier\PlatformDirt\DecoHill2", @"\Media\Modifier\StadiumOnTerrain\DecoHill2" },
    { @"Stadium\Media\Modifier\PlatformIce\Deco", @"\Media\Modifier\StadiumOnTerrain\Deco" },
    { @"Stadium\Media\Modifier\PlatformIce\DecoHill", @"\Media\Modifier\StadiumOnTerrain\DecoHill" },
    { @"Stadium\Media\Modifier\PlatformIce\DecoHill2", @"\Media\Modifier\StadiumOnTerrain\DecoHill2" },
    { @"Stadium\Media\Material\TrackWallClips", @"\Media\Modifier\StadiumOnTerrain\TrackWallClipsInWorld" },
    { @"Stadium\Media\Material\TrackBorders", @"\Media\Modifier\StadiumOnTerrain\TrackBordersInWorld" },
    { @"Stadium\Media\Material\TrackBordersOff", @"\Media\Modifier\StadiumOnTerrain\TrackBordersOffInWorld" },
    { @"Stadium\Media\Material\DecalPaintSponsor4x1D", @"\Media\Modifier\StadiumOnTerrain\DecalPaintSponsor4x1D" },
    { @"Stadium\Media\Material\DecalPaint2Sponsor4x1D", @"\Media\Modifier\StadiumOnTerrain\DecalPaint2Sponsor4x1D" },
    { @"Stadium\Media\Material\DecalPaintSponsor4x1NoColorizeD", @"\Media\Modifier\StadiumOnTerrain\DecalPaintSponsor4x1NoColorizeD" },
    { @"Stadium\Media\Material\DecalPaint2Sponsor4x1NoColorizeD", @"\Media\Modifier\StadiumOnTerrain\DecalPaint2Sponsor4x1NoColorizeD" },
    { @"Stadium\Media\Material\TrackWall", @"\Media\Modifier\StadiumOnTerrain\TrackWallInWorld" },

    { @"Deco", @"\Media\Modifier\StadiumOnTerrain\Deco" },
    { @"DecoHill", @"\Media\Modifier\StadiumOnTerrain\DecoHill" },
    { @"DecoHill2", @"\Media\Modifier\StadiumOnTerrain\DecoHill2" },
    { @"PlatformDirt_Deco", @"\Media\Modifier\StadiumOnTerrain\Deco" },
    { @"PlatformDirt_DecoHill", @"\Media\Modifier\StadiumOnTerrain\DecoHill" },
    { @"PlatformDirt_DecoHill2", @"\Media\Modifier\StadiumOnTerrain\DecoHill2" },
    { @"PlatformIce_Deco", @"\Media\Modifier\StadiumOnTerrain\Deco" },
    { @"PlatformIce_DecoHill", @"\Media\Modifier\StadiumOnTerrain\DecoHill" },
    { @"PlatformIce_DecoHill2", @"\Media\Modifier\StadiumOnTerrain\DecoHill2" },
    { @"TrackWallClips", @"\Media\Modifier\StadiumOnTerrain\TrackWallClipsInWorld" },
    { @"TrackBorders", @"\Media\Modifier\StadiumOnTerrain\TrackBordersInWorld" },
    { @"TrackBordersOff", @"\Media\Modifier\StadiumOnTerrain\TrackBordersOffInWorld" },
    { @"DecalPaintSponsor4x1D", @"\Media\Modifier\StadiumOnTerrain\DecalPaintSponsor4x1D" },
    { @"DecalPaint2Sponsor4x1D", @"\Media\Modifier\StadiumOnTerrain\DecalPaint2Sponsor4x1D" },
    { @"DecalPaintSponsor4x1NoColorizeD", @"\Media\Modifier\StadiumOnTerrain\DecalPaintSponsor4x1NoColorizeD" },
    { @"DecalPaint2Sponsor4x1NoColorizeD", @"\Media\Modifier\StadiumOnTerrain\DecalPaint2Sponsor4x1NoColorizeD" },
    { @"TrackWall", @"\Media\Modifier\StadiumOnTerrain\TrackWallInWorld" }
};
Gbx.LZO = new Lzo();
foreach (string vista in vistas)
{
    foreach (var file in allFiles)
    {
        CMwNod node;
        try
        {
            node = Gbx.ParseNode(file);
        }
        catch (Exception e) { Console.WriteLine(e.Message); continue; }

        if (node is CGameItemModel item)
        {
            List<string> pathParts = Path.GetDirectoryName(file)!.Split(Path.DirectorySeparatorChar).ToList();
            string? itemsRoot = null;
            StringBuilder path = new();
            List<string> itemDirParts = new();
            foreach (string part in pathParts)
            {
                path.Append(part+'\\');
                itemDirParts.Add(part);
                if (part == "Items") {
                    itemsRoot = path.ToString();
                    break;
                }
            }
            pathParts.RemoveAll(itemDirParts.Contains);
            if (itemsRoot == null)
            {
                Console.WriteLine("Couldn't find the \"Items\" folder. Try moving your items to your in-game items folder and try again");
                Exit();
            }
            path.Append(vista + '\\');
            foreach (string part in pathParts)
            {
                path.Append(part + '\\');
            }
            string savePath = Path.Combine(path.ToString(), Path.GetFileName(file));
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            if (item.EntityModel is CGameCommonItemEntityModel importedItem)
            {
                foreach (CPlugSolid2Model.Material material in importedItem.StaticObject.Mesh.CustomMaterials)
                {
                    if (!vistasMaterialsMapping.ContainsKey(material.MaterialUserInst.Link)) continue;
                    material.MaterialUserInst.Link = vista + vistasMaterialsMapping[material.MaterialUserInst.Link];
                    if (material.MaterialUserInst.Link.Contains("Deco"))
                    {
                        if (
                            material.MaterialUserInst.SurfacePhysicId == CPlugSurface.MaterialId.Grass ||
                            material.MaterialUserInst.SurfacePhysicId == CPlugSurface.MaterialId.Sand ||
                            material.MaterialUserInst.SurfacePhysicId == CPlugSurface.MaterialId.Snow
                        )
                        {
                            switch (vista)
                            {
                                case "RedIsland":
                                    {
                                        material.MaterialUserInst.SurfacePhysicId = CPlugSurface.MaterialId.Sand;
                                        break;
                                    }
                                case "GreenCoast":
                                case "BlueBay":
                                    {
                                        material.MaterialUserInst.SurfacePhysicId = CPlugSurface.MaterialId.Grass;
                                        break;
                                    }
                                case "WhiteShore":
                                    {
                                        material.MaterialUserInst.SurfacePhysicId = CPlugSurface.MaterialId.Snow;
                                        break;
                                    }
                            }
                        } else //Use "Penalty" material (texture from stadium)
                        {
                            if (material.MaterialUserInst.Link.Contains("DecoHill2"))
                            {
                                material.MaterialUserInst.Link = material.MaterialUserInst.Link.Replace("DecoHill2", "Penalty");
                            }
                        }
                        
                    }

                }
                item.Save(savePath);
                Console.WriteLine(savePath);
            } else if (item.EntityModelEdition is CGameCommonItemEntityModelEdition mmItem)
            {
                Console.WriteLine("Mesh modeler items not supported, skipping...");
                continue;
            }

        }
    }
}
Console.WriteLine("Done!");
Exit();

static void Exit()
{
    Console.WriteLine("Press any key to exit.");
    Console.ReadKey();
}