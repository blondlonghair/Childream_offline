using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;

namespace _2_Scripts
{
    public class test : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer image;
        [SerializeField] private AssetReference assetReference;
        [SerializeField] private AssetReferenceGameObject assetReferenceGameObject;
        [SerializeField] private List<AssetReferenceSprite> AssetReferenceSprite;

        [SerializeField] private AssetLabelReference assetLabelReference;
        private IList<IResourceLocation> _locations;

        public void AddressablesPrefab()
        {
            // 메모리 로드 구문 없이 바로 Instantiate
            Addressables.InstantiateAsync(assetReferenceGameObject);
        }
 
        public void AddressablesScene()
        {
            // 메모리 로드 구문 없이 바로 씬 로드
            assetReference.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
 
        public void AddressablesSprite()
        {
            // 메모리에 로드 후 콜백 호출
            foreach (var assetReferenceSprite in AssetReferenceSprite)
            {
                assetReferenceSprite.LoadAssetAsync().Completed += OnSpriteLoaded;
            }
        }
 
        // 콜백 함수
        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
        {
            image.sprite = handle.Result;
        }
    }
}