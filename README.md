Cix
===

A C-based language without header files.

1. What is Cix?
Cix (pronounced "six") is a programming language deriving mostly from C, except with the exclusion of header files and certain syntactical constructs. Cix supports certain preprocessor directives (#define and conditionals based on it, #include), structures, unions, and functions.

2. What's in this repository?
This repository contains a formal grammar (albeit in no standard form) that helps to define the structures of the Cix grammar. It also contains a C#/.Net application that tokenizes and parses Cix files into Abstract Syntax Trees (ASTs) that can then be processed by other programs - for instance: compilers, static anaylzers, or interpreters.
