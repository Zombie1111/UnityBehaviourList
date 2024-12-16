
<h1 align="center">Unity Behaviour List By David Westberg</h1>

## Table of Contents
- [Overview](#overview)
- [Key Features](#key-features)
- [Instructions](#instructions)
- [Documentation](#documentation)
- [License](#license)

## Overview
A Behaviour List is a list of states and conditions to change the state. Each state can be assigned a script (The behaviour of the state) and the conditions are a list of scripts. Like a semi-visual state machine.

<img src="https://i.postimg.cc/rpKNwcyq/image.png" width="75%" height="75%" alt="Image of Behaviour List"/>

## Key Features
<ul>
<li>Create your own states and conditions by simply inherit a abstract class</li>
<li>Share data between states and conditions through the Behaviour List ListData object</li>
<li>States can be updated from any thread (Although not at the same time)</li>
<li>Custom Behaviour List inspector</li>
</ul>

## Instructions
**Requirements** (Should work in other versions and render pipelines)
<ul>
<li>Unity 2023.2.20f1 (Built-in)</li>
</ul>

**General Setup**

<ol>
  <li>Download and copy the <code>Core</code>, <code>Extras(Optional)</code> and <code>_Demo(Optional)</code> folders into an empty folder inside your <code>Assets</code> directory</li>
  <li>Add a serialized <code>behLists.BehaviourList</code> variable to any of your scripts to create a new Behaviour List</li>
  <li>In your script, call Init(), TickRoot() and Destroy() on your behLists.BehaviourList</li>
  <li>Configure your BehaviourList in the inspector. Create scripts with a class that inherit from <code>ListData, BranchBehaviour and LeafCondition</code> (In behLists namespace) if needed</li>
  <li>See <code>_Demo/ExampleBehaviourScript.cs</code> for code example</li>
</ol>

## Documentation
Most functions are documented and all parameters visible in the Unity inspector have tooltips

See `Extras/Chance.cs` for LeafCondition code example

See `_Demo/ExampleAiData.cs` for ListData code example

See `_Demo/ExampleBranchBehIdle.cs` for BranchBehaviour code example

The `_Demo/` folder contains more pratical exampels

## License
UnityBehaviourList © 2024 by David Westberg is licensed under MIT - See the `LICENSE` file for more details.

