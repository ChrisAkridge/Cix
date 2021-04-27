# Cix: IronArc Control Flow Graph to Assembly Code

## Problem

Given a control flow graph made of instruction vertices connected in sequence, with some vertices connected to multiple other vertices, and some vertices connected cyclically, generate a string containing an IronArc assembly file.

Each vertex may generate a string representing its instruction. Some edges may also generate instructions of their own representing jumps to their targets. Some instructions are marked as being jump targets - these will generate labels alongside their instructions.

The only constraint on instruction order in the final generated assembly file is that instructions connected by direct flow must appear adjacent to each other. Otherwise, instructions can be in any order.

Edges hold on to their source and their destination. We can combine this with a new property, `int VisitCount`, which is incremented by 1 each time the vertex is visited. Each generation of the assembly file starts with the start vertex and records its `VisitCount`, marking that as the "unvisited" indicator.

Starting the the start vertex:

1. Create a `List<string>` representing the generated instruction strings for the vertices processed in this method.
2. Create a `List<List<string>>` representing the generated instruction string lists for each outbound non-direct-flow control flow graph.
3. Does `VisitCount == UnvisitedIndicator`?
	- Yes:
		1. Generate the instruction for the vertex and add it to the list.
		2. For each outbound non-direct-flow edge, generate a jump instruction, getting the label by following the edge to the target vertices, and add them to the list.
		3. Increment `VisitCount`.
		4. For each outbound non-direct-flow edge, recurse into step 1. 
		5. How many outbound direct-flow edges are there?
			- Two or more: Throw an exception.
			- One: Set the current vertex to the target of the edge, then `continue` to step 3.
			- Zero: The loop is complete. Go to step 4.
	- No: This graph has its instructions already generated. Return an empty list.
4. Perform `SelectMany` on the non-empty list of lists from step 2 and concatenate it with the list from step 1.
5. Return the generated list.