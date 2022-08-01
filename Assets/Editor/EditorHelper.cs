using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class EditorHelper : MonoBehaviour 
{
    [MenuItem("EditorHelper/SliceSprites")]
    static void SliceSprites()
    {
        // 자를 스프라이트 가로, 세로 높이
        int sliceWidth = 64;
        int sliceHeight = 64;

        // 스프라이트가 포함된 폴더 경로
        string folderPath = "ToSlice";

        Object[] spriteSheets = Resources.LoadAll(folderPath, typeof(Texture2D));
        Debug.Log("spriteSheets.Length: " + spriteSheets.Length);

        for(int i = 0; i < spriteSheets.Length; i++)
        {
            Debug.Log("spritesSheets[i]: " + spriteSheets[i]);

            string path = AssetDatabase.GetAssetPath(spriteSheets[i]);
            TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
            ti.isReadable = true;
            ti.spriteImportMode = SpriteImportMode.Multiple;

            List<SpriteMetaData> newData = new List<SpriteMetaData>();

            Texture2D spriteSheet = spriteSheets[i] as Texture2D;

            for(int j = 0; j < spriteSheet.width; j += sliceWidth)
            {
                for(int k = spriteSheet.height; k > 0; k -= sliceHeight)
                {
                    SpriteMetaData smd = new SpriteMetaData();
                    smd.pivot = new Vector2(0.5f, 0.5f);
                    smd.alignment = 9;
                    smd.name = (spriteSheet.height - k) / sliceHeight + ", " + j / sliceWidth;
                    smd.rect = new Rect(j, k - sliceHeight, sliceWidth, sliceHeight);

                    newData.Add(smd);
                }
            }

            ti.spritesheet = newData.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        Debug.Log("Done Slicing!");
    }
}