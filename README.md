v# NoxLayers
Simple utiltiy scripts helpful when working with Unity layers.

It's purpose is to provide a refactoring and robust layer solution avoiding bugs because of bit shifts, layer math and comparison.
Also it avoids having to look into the defined layers for each code review, as meaningful variable names instead of ints can be used.


# Features
* Supports Layer Masks
* Provides operator overloads (+,-,|,!=,==,)
* Renaming robust
* Splits variable naming from layer naming
* Provides implicit type conversion between Layer, Mask and int
* Provides a fluent syntax
* Provides Enum to String conversion

## Usage
It is recommended to rename the variable names of the enum values in `Layer.cs` to match the layer name. This is refactoring proof.
```
// e.g. rename Layer.L13 to Layer.Enemy
gameObject.layer == Layer.Enemy 
var go = this.gameObject;
var layer = (Layer) go.layer;
var mask = new Mask(layer);
if (mask.Contains(layer))
{
	// mask has the layer bit set (and maybe other bits as well)
}

if (mask.Is(layer))
{
	// mask exactly is defined by the layer bit
}
```
Also rename the display name for your layer in `LayerMaskEnums.cs`. This helps to decouple the layer display name and the variable name.

# Installation
* Import the noxlayers-<version>.unitypackage
* Move the folder NoxLayers/* folder to any assembly linked to any other you intend to use logging 
  * e.g. Plugins/* if you do not use assembly definitions

# Best Practices
* Work with Layer whenever possible, and use casts to convert int to Layer `gameObject.layer` 
```
var layer = (Layer) gameObject.layer;
```
* Define static Masks for commonly used layer masks and reuse them
```
public static Mask CollidesWithPlayer = new Mask(Layer.Floor|Layer.Wall|Layer.Enemy|...);
```

# Disclaimer
This project is intented for personal use only at the moment and is provided AS IS. Use it at your own risk.
If others find it helpful - great, if not fine.