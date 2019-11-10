﻿using System.Collections;
using System.Collections.Generic;
using DCL.Helpers;
using NUnit.Framework;
using Tests;
using UnityEngine;
using UnityEngine.TestTools;

namespace AvatarEditorHUD_Tests
{
    public class AvatarEditorHUDController_Mock : AvatarEditorHUDController
    {
        public AvatarEditorHUDController_Mock(UserProfile userProfile, WearableDictionary catalog) : base(userProfile, catalog) { }

        public AvatarEditorHUDModel myModel => model;
        public AvatarEditorHUDView myView => view;
        public string[] myCategoriesThatMustHaveSelection => categoriesThatMustHaveSelection;
        public string[] myCategoriesToRandomize => categoriesToRandomize;
    }

    public class WearableItemsShould : TestsBase
    {
        private UserProfile userProfile;
        private AvatarEditorHUDController_Mock controller;
        private WearableDictionary catalog;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return InitScene();

            userProfile = ScriptableObject.CreateInstance<UserProfile>();
            userProfile.UpdateData(new UserProfileModel()
            {
                name = "name",
                email = "mail",
                avatar = new AvatarModel()
                {
                    bodyShape = WearableLiterals.BodyShapes.FEMALE,
                    wearables = new List<string>()
                        { }
                }
            });

            catalog = AvatarTestHelpers.CreateTestCatalog();
            controller = new AvatarEditorHUDController_Mock(userProfile, catalog);
        }

        [TearDown]
        public void TearDown()
        {
            controller.CleanUp();
        }

        [Test]
        public void BeAddedWhenEquiped()
        {
            var sunglassesId = "dcl://base-avatars/black_sun_glasses";
            var sunglasses = catalog.Get(sunglassesId);

            controller.WearableClicked(sunglassesId);
            Assert.IsTrue(controller.myModel.wearables.Contains(sunglasses));
        }

        [Test]
        public void BeReplacedByGeneralReplaces()
        {
            var sunglassesId = "dcl://base-avatars/black_sun_glasses";
            var sunglasses = catalog.Get(sunglassesId);
            var bandanaId = "dcl://base-avatars/blue_bandana";
            var bandana = catalog.Get(bandanaId);

            bandana.replaces = new [] { sunglasses.category };
            controller.WearableClicked(sunglassesId);
            controller.WearableClicked(bandanaId);

            Assert.IsTrue(controller.myModel.wearables.Contains(bandana));
            Assert.IsFalse(controller.myModel.wearables.Contains(sunglasses));
        }

        [Test]
        public void NotBeReplacedByWrongGeneralReplaces()
        {
            var sunglassesId = "dcl://base-avatars/black_sun_glasses";
            var sunglasses = catalog.Get(sunglassesId);
            var bandanaId = "dcl://base-avatars/blue_bandana";
            var bandana = catalog.Get(bandanaId);

            bandana.replaces = new [] { "NonExistentCategory" };
            controller.WearableClicked(sunglassesId);
            controller.WearableClicked(bandanaId);

            Assert.IsTrue(controller.myModel.wearables.Contains(bandana));
            Assert.IsTrue(controller.myModel.wearables.Contains(sunglasses));
        }

        [Test]
        public void BeReplacedByOverrideReplaces()
        {
            var sunglassesId = "dcl://base-avatars/black_sun_glasses";
            var sunglasses = catalog.Get(sunglassesId);
            var bandanaId = "dcl://base-avatars/blue_bandana";
            var bandana = catalog.Get(bandanaId);

            bandana.GetRepresentation(userProfile.avatar.bodyShape).overrideReplaces = new [] { sunglasses.category };
            controller.WearableClicked(sunglassesId);
            controller.WearableClicked(bandanaId);

            Assert.IsTrue(controller.myModel.wearables.Contains(bandana));
            Assert.IsFalse(controller.myModel.wearables.Contains(sunglasses));
        }

        [Test]
        public void NotBeReplacedByWrongOverrideReplaces()
        {
            var sunglassesId = "dcl://base-avatars/black_sun_glasses";
            var sunglasses = catalog.Get(sunglassesId);
            var bandanaId = "dcl://base-avatars/blue_bandana";
            var bandana = catalog.Get(bandanaId);

            bandana.GetRepresentation(WearableLiterals.BodyShapes.MALE).overrideReplaces = new [] { sunglasses.category };
            controller.WearableClicked(sunglassesId);
            controller.WearableClicked(bandanaId);

            Assert.IsTrue(controller.myModel.wearables.Contains(bandana));
            Assert.IsTrue(controller.myModel.wearables.Contains(sunglasses));
        }
    }
}