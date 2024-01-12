using UnityEngine;

// ワールドを生成するクラス
public class WorldGenerater : MonoBehaviour
{
    [SerializeField]
    private WorldDataBunker _worldDataBunker;
    
    [SerializeField]
    private MaterialBunker _materialBunker;
    [SerializeField]
    private ItemBunker _itemBunker;
    [SerializeField] 
    private Transform _worldParent;


    public void WorldGenerate()
    {  
        for (int i = 0; i < _worldDataBunker.WorldObjCount; i++)
        {
            // 生成するオブジェクト
            int insKind = FindItemKind(_worldDataBunker.ObjShapeList[i]);

            if (insKind == -1)
            {
                Debug.LogError("Invalid tag: " + _worldDataBunker.ObjShapeList[i]);
                continue;
            }

            GameObject insObj = Instantiate(_itemBunker.ItemObject[insKind], _worldDataBunker.ObjPosList[i], _worldDataBunker.ObjRotList[i], _worldParent);

            // マテリアルの選定
            Material assignMat = GetAssignedMaterial(_worldDataBunker.ObjMatList[i]);

            if (assignMat == null)
            {
                Debug.LogError("Invalid material index: " + _worldDataBunker.ObjMatList[i]);
                continue;
            }

            Renderer renderer = insObj.GetComponent<Renderer>();
            renderer.GetComponent<Renderer>().material = assignMat;

            // IDの割り振り
            ObjectID objectID = insObj.GetComponent<ObjectID>();
            objectID.SetObjID(i);
        }
    }

    int FindItemKind(string tag)
    {
        switch (tag)
        {
            case "Cube":
                return 0;
            case "HalfCube":
                return 1;
            case "Step":
                return 2;
            case "SmallCube":
                return 3;
            default:
                return -1; // 無効なタグの場合
        }
    }

    Material GetAssignedMaterial(int targetNum)
    {        
        int keyNum = targetNum + 1;
        if (keyNum < MaterialBunker.MATERIAL_AMOUNT)
        {
            string tmpKey = MaterialBunker.KEY_NAME + keyNum;
            return _materialBunker.ImageMaterials.ContainsKey(tmpKey) ? _materialBunker.ImageMaterials[tmpKey] : null;
        }

        int defaultMat = targetNum - MaterialBunker.MATERIAL_AMOUNT;
        if (defaultMat >= 0 && defaultMat < _materialBunker.DefaultMat.Length)
        {
            return _materialBunker.DefaultMat[defaultMat];
        }
        return null;  
    }
}
