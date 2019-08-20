using DCL;
using DCL.Helpers;
using DCL.Models;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TexturesTests : TestsBase
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TextureCreateAndLoadTest()
        {
            yield return InitScene();

            DCLTexture dclTexture = TestHelpers.CreateDCLTexture(scene,
                TestHelpers.GetTestsAssetsPath() + "/Images/avatar.png",
                DCLTexture.BabylonWrapMode.CLAMP,
                FilterMode.Bilinear);

            yield return dclTexture.routine;

            Assert.IsTrue(dclTexture.texture != null, "Texture didn't load correctly?");
            Assert.IsTrue(dclTexture.unityWrap == TextureWrapMode.Clamp, "Bad wrap mode!");
            Assert.IsTrue(dclTexture.unitySamplingMode == FilterMode.Bilinear, "Bad sampling mode!");

            dclTexture.Dispose();

            yield return null;
            Assert.IsTrue(dclTexture.texture == null, "Texture didn't dispose correctly?");
        }

        [UnityTest]
        public IEnumerator TextureAttachedGetsReplacedOnNewAttachment()
        {
            yield return InitScene();

            yield return TestHelpers.TestAttachedSharedComponentOfSameTypeIsReplaced<DCLTexture.Model, DCLTexture>(
                scene, CLASS_ID.TEXTURE);
        }

        [UnityTest]
        public IEnumerator Texture_OnReadyBeforeLoading()
        {
            yield return InitScene();

            DCLTexture dclTexture = TestHelpers.CreateDCLTexture(scene,TestHelpers.GetTestsAssetsPath() + "/Images/avatar.png");
            bool isOnReady = false;
            dclTexture.CallWhenReady((x) => { isOnReady = true; });

            Assert.IsTrue(isOnReady); //DCLTexture is ready on creation
        }

        [UnityTest]
        public IEnumerator Texture_OnReadyWaitLoading()
        {
            yield return InitScene();

            DCLTexture dclTexture = TestHelpers.CreateDCLTexture(scene,TestHelpers.GetTestsAssetsPath() + "/Images/avatar.png");
            bool isOnReady = false;
            dclTexture.CallWhenReady((x) => { isOnReady = true; });
            yield return dclTexture.routine;

            Assert.IsTrue(isOnReady);
        }

        [UnityTest]
        public IEnumerator Texture_OnReadyAfterLoadingInstantlyCalled()
        {
            yield return InitScene();

            DCLTexture dclTexture = TestHelpers.CreateDCLTexture(scene,TestHelpers.GetTestsAssetsPath() + "/Images/avatar.png");
            yield return dclTexture.routine;

            bool isOnReady = false;
            dclTexture.CallWhenReady((x) => { isOnReady = true; });
            Assert.IsTrue(isOnReady);
        }
    }
}