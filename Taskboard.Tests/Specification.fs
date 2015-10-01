[<AutoOpen>]
module Specification

open System
open Xunit
open Swensen.Unquote

let Given events =
    List.fold (fun s e -> evolve e s) Board.initial events

let When command state =
    command, state

let Then expected (command, state) =
    let actual = handle command state
    test<@ expected = actual @>

let ThenThrows (command, state) =
    let act = Action (function _ -> handle command state |> ignore)
    Assert.Throws(act) |> ignore
    ()
