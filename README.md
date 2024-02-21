# unity2d-ship-shooter-defaultecs

A WIP attempt to implement a simple game using [DefaultEcs](https://github.com/Doraku/DefaultEcs).

## Design

### Assets

Asset registries shall hold all unity prefabs references with guid and name.
Any entity created with an Asset component shall be processed by an Asset system, which shall fetch the prefab from registries using name and/or guid, and generate a display component holding an instantiated game object.

Asset registries should be addressable and configured to be loaded dynamically from Unity Content Network, thus updatable after deployment.

Configuration registries should behave the same but without prefabs. It should help data configuration edition, also known as initial data, in addition to Entity Component System data design that can be considered runtime data. Minor edition tools added to help with edition of registries.

An implementation of SerialisableGuid using two 64-bit <c>ulong</c> is chosen (another choice is using string) to help reduce potential conflict of ids when team adding new elements to the same registry. SerializableGuid<T> should be used later on for strongly typed identifiers since it complexifies a bit the editor tool.

### GUI

World has one Device component that shall be added to every world entity. Device at any momment will have one active Screen.
One Screen component is added to every entities that shall be displayed if the screen is active, else disabled thus not visible.

Some Unity MonoBehaviors such as ButtonHelper and LabelHelper are added to help with rendering interactive UI elements until a proper data binding is implemented.

### Gameplay

Quick and dirty systems coded to validate the concepts first and may contain bugs and obviously can be optimized.

## Next steps

* Complete the gameplay
* Fix the GUI
* Check why webgl does not work, maybe because of unsafe code for SerializableGuid (switch to using string which is less efficient but does not use unsafe code)
* Fix shortcuts that go against the design, such as storing parameters in model static fields, or put all things in Bootstrap
* Make use of strongly typed identifiers and find a strategy to have one entry for all registries edition (automagically create asset files per type) with filters, etc...
* Make a multiplayer game
 