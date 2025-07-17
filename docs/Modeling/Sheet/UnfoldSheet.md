---
uid: 87d3ecca-434c-474d-befd-47f1bb83370e
title: Unfold Sheet
icon: UnfoldSheet.svg
---
Unfolds a sheet with bend flanges with respect to the material compression.

## Properties
Select Start Face
:   Enables the selection of a planar face as the starting point for unfolding. The start face defines the plane into which the shape is unfolded and is not changed or moved. This allows you to control the direction in which the model is unfolded, e.g., horizontally or vertically. In some situations, the automatic determination of the start face does not work well, in which case this option allows you to make a better selection.
    The “Automatic” button can be used to delete a previous manual selection and automatically determine the start area again.

## Remarks
The unfolding algorithm searches for bend sections created by former modifiers. This bend sections are replaced with straight sections.
The length of the straight sections is calculated by taking the material compression into account. This is done by applying the _k-Factor_, which is calculated based on the bend radius and the material thickness.

> [!IMPORTANT]
> If the bend sections can not be discovered correctly, make sure that they are not modified in a way which destroys it's topology or geometry. 
> Furthermore, selecting a different starting surface can help you avoid difficult sections and still find the right bend sections.

The bend sections are determined using contiguous surfaces. If a shape consists of several unconnected solids — e.g. because a shape has been duplicated — then a separate _UnfoldSheet_ modifier must be added for each solid that is to be unfolded, and the appropriate start face must be selected manually for each one.

The modifier can also unfold imported solids as long as they have the topological features required for recognizing the bend sections. These include:
* The top and bottom of the bend sections must consist of parallel cylindrical surfaces.
* The side edges of the bend sections must consist of a continuous circle arc or elliptical arc.
* Surfaces and edges made of b-splines (NURBS) are not supported.

## Unfolding a bended sheet
1. Select the solid to be unfolded.
2. Select __Unfold Sheet__ from ribbon menu.
3. Select the base face to which the sheet will be flattened. If the base face can be detected automatically, this step is being skipped.

## See Also
[](xref:5f9b1a87-60f9-448a-860a-567eb18473c8)
