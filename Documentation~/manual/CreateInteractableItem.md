---
uid: create_interactable_item
title: Create a Interactable Item
---
# Create a interactable item

The easiest way to create a interactable item is to right click in the hierarchy window and in the menu select:

`MoxieJam->StoryFramework->Create Interactable Item`

![Right click in hierarchy and select MoxieJam/StoryFramework/Create Interactable Item](../resources/images/CreateInteractableItem.png)

A new window will now open with settings for the interactable item:

1. Name of the game object
2. What image to use
3. Weather it's possible to pick up the item or not.
4. A `Inventory Item` to associate with this object. This is the actual item that will be picked up in to the inventory. If you have not created the `Inventory Item` before hand, you can press `Create Item` to create one now.

Once you are satisfied with your settings you click `Create` and it will be available in the scene. If you have changed your mind about creating the object just click cancel.

![Configure item in window](../resources/images/CreateInteractableItem2.png)

You have now created a interactable item, congratulations! Select the newly created object in the hierarchy window and take a look in the inspector window. This is all the components of the intractable item you just created:

1. `Transform` decided where to place the object in the scene. You can also use it to rotate or change the size of the object.
2. `Box Collider 2D` is used to decide the clickable/mouser-over area of the object.
3. `Sprite Renderer` like the name implies is used to render (draw) the sprite. You can assign a `Sprite` (image) here and do things such as flip it, and change what layer and order it's being drawn.
4. See <xref:component_persistent_object>.
5. See <xref:component_interactable_item>
6. The last component is the material used by the `Sprite Renderer` to draw the image.

![A Interactable Item](../resources/images/InteractableItem.png)
