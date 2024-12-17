---
uid: 55fc2982-4f52-4c9d-8e75-b1b100fd53b0
title: Reference
---
Create a reference shape to start a new body on a shape of another body.

## Properties

Use Shape
:   The shape of the referenced body to be used.
    * __Top Shape__: Always the topmost shape of the shape stack so that the reference is identical to the referenced body. 
    * __Current Shape__: Adopts the currently activated shape on the shape stack of the referenced body, regardless of which additional modifiers exist or added later.

## Remarks

Each time the shape of the original body is changed, the shape of the reference shape also changes. Regardless of this, the body of the referenced shape can accept new modifiers in order to further edit the inherited shape.

When the original shape or it's body is deleted, the reference shape will be substituted by a solid consisting of the latest geometry before the shape was deleted.

## Creating a Reference

1. Select the original body.
2. If the reference should not use the current shape, set the desired shape in the shape stack as current.
3. Select **Create Reference** from ribbon menu in the **Edit** tab.
4. The reference is created and selected.
