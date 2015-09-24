[<AutoOpen>]
module Specification

let Given events =
    List.fold (fun s e -> evolve e s) Board.initial events
