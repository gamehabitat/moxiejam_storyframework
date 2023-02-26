---
uid: install_framework
title: Install MoxieJam Story Framework
---
# Installation

This document will guide you through creating a new Unity-project and installing the framework.

## Prerequisites

This package requires Unity version 2021.3.19f1 (newer version might potential work but can't be guaranteed).

## Create a new project

Open Unity Hub and click on `New Project`

![Click on new project in Unity Hub](../resources/images/install/NewProject.png)

In this new screen you customize your new project:

* Make sure that the Editor Version at the top of the screen is set to the correct version (see [Prerequisites](#prerequisites) section)
* Select the core template named `2D`.
* Name your project in the `Project Settings` section.
* Change the `Location` to where you would like to save your project.

When you are satisfied with settings press `Create project`.

![Configure project settings](../resources/images/install/NewProject2.png)

Unity will now open your newly created project (if this did not happen, open the project through the Unity Hub).

![Unity window is open](../resources/images/install/Unity1.png)

Now you are ready to install the MoxieJam Story Framework!

## Install the MoxieJam Story Framework

In the `Window` menu in unity click on `Package Manager`.

![In the `Window` menu in unity click on `Package Manager`.](../resources/images/install/UnityAddPackage1.png)

A new window with the `Package Manager` is now opened. Press the `+` menu in the top left corner and click on `Add package from git URL...`.

![Press the `+` menu in the top left corner and click on `Add package from git URL...`](../resources/images/install/UnityAddPackage2.png)

In this new window paste in the MoxieJam Story Frameworks URL:

`https://github.com/gamehabitat/moxiejam_storyframework.git`

![Unity window is open](../resources/images/install/UnityAddPackage3.png)

Unity's package manager will now start downloading and installing the package. Once it is completed you will see a green checkmark to the right of the version number of the package.

### Install TextMeshpro resources

Once the package is installed a window named `TMP Importer` might appear and ask you to `Import TMP Essentials` go ahead and do this as it will add some needed assets to make texts in the game work.

![Import TMP Essentials](../resources/images/install/ImportTMP.png)

If this window did not open up, instead in the `Window` menu under the sub-menu `TextMeshPro` click on `Import TMP Essential Resources` to import them.

![Click on menu Window/TextMeshPro/Import TMP Essentials](../resources/images/install/ImportTMP2.png)

When you start importing a new window will open up. Click `Import` in the lower right corner of it to start importing.

![Import TMP window](../resources/images/install/ImportTMP3.png)

The TextMeshPro resources is now installed.

### After installation

When the package is installed you can optionally install one of the [sample projects](#install-a-sample-project).

![Unity window is open](../resources/images/install/UnityAddPackage4.png)

## Install a sample project

The framework comes with two sample projects to help you get started and to demonstrate what can be created using the MoxieJam Story framework.

If you want a smaller example to get started quickly, you can install the single scene sample project:

<xref:install_simple_project>

If you are more interested in a larger sample with multiple scenes with interactions between the scenes you can install the sample game:

<xref:install_sample_project>
