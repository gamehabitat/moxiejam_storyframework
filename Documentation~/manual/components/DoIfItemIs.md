---
uid: component_do_if_item_is
title: Do If Item Is
---
# Do If Item Is

![Do If Item Is component](../../resources/images/components/DoIfItemIs.png)

The Do If Item Is component is useful for drop target actions to do conditional tests on the dropped item.

If you want to use this together with a drop (multi) item target, remember to select the dynamic version of TryDo in order for the drop target to pass the dropped item on to this component:

![Use dynamic TryDo](../../resources/images/components/DoIfItemIsSelectDynamic.png)

## Settings

### Required item

The item to test against.

## Events

The component provide some useful events you can use to make something happen when interacting with the component.

### On Item Accepted

This event happens if the user have passed the require item.

### On Item Rejected

This event happens if the user passed an item on this object and it was the wrong item.

## Methods to use in events

* `TryDo` takes a inventory item and test if this item is the required item and depending on if it is it will do one of the two events specified above. 

## Related components

* <xref:component_drop_multi_item_target>

