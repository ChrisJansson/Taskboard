[<AutoOpen>]
module Specification

open Swensen.Unquote

let Given events =
    List.fold (fun s e -> evolve e s) Board.initial events

let When command state =
    handle command state

let Then expected actual =
    test<@ expected = actual @>
