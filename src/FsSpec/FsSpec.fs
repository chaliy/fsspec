namespace FsSpec

exception AssertFailed of string

[<AutoOpen>]
module SpecHelpers =
           
    /// Sub topic of the spec, aggregate spec clauses
    let describe desc (f: unit -> unit) =
        printfn "%s" desc
        f()  
        printfn "\r\n"
            
    /// Describe single spec clause
    let it desc (f: unit -> unit) =        
        try
           do f()
           printfn "\t\t- %s - OK" desc
        with
            | AssertFailed x -> printfn "\t\t- %s - %s" desc x
            | x -> printfn "\t\t- %s - %s\r\n%s" desc x.Message (x.ToString())               

    // Spec Builder

    /// Specification
    type Spec = unit -> unit    
     
    type SpecBuilder() =
                
        member b.Delay(f : unit -> Spec) = (fun() -> f()())
        member b.Zero() = (fun() -> ())
        member b.Using(g : System.IDisposable, e) = () // Ignore for now... TODO Really need this!
    
    let spec = new SpecBuilder()

    // Asserts...
    type System.Object with

        member x.should_be_equal_to expected =
            if x <> expected then
                raise (AssertFailed(sprintf "Not really equal. Expected to be %s, but was %s" (expected.ToString()) (x.ToString())))    

        member x.should_not_be_null =                
            if obj.Equals(x, null) then
                raise (AssertFailed(sprintf "Bumm, unexpected NULL"))    

        member x.should_be_null =
            if obj.Equals(x, null) = false then
                raise (AssertFailed(sprintf "Damn! It does not NULL"))

    type System.Boolean with        
        member x.should_be_true =
            if (x <> true) then
                raise (AssertFailed(sprintf "TRUE is expected."))

        member x.should_be_false =
            if (x <> false) then
                raise (AssertFailed(sprintf "FALSE is expected."))

    type System.Collections.Generic.IEnumerable<'T> with        
        member x.should_not_be_empty =
            if x |> Seq.isEmpty then
                raise (AssertFailed(sprintf "Non empty sequence is expected."))

        member x.should_be_empty =
            if not (x |> Seq.isEmpty) then
                raise (AssertFailed(sprintf "Empty sequence is expected."))

        member x.should_has_items_of e =
            let size = x |> Seq.length
            if size <> e then
                raise (AssertFailed(sprintf "Sequence expected to have %i items. But was %i" e size))

    let justfail() =
        raise (AssertFailed(sprintf "Just failed!"))

    let should_fail (s : unit -> unit) =
        try
           do s()
           raise (AssertFailed(sprintf "Nothing failed!"))
        with | _ -> () 

    


/// All about running specs
module Runner =
    open System.Reflection    
    open Microsoft.FSharp.Metadata
    open Microsoft.FSharp.Reflection
    open System.Text.RegularExpressions

    let internal printTitle (title : string) = 
            let sep = (new System.String('-', title.Length + 4))
            printfn "\r\n%s\r\n  %s  \r\n%s\r\n"  sep title sep 

    let specRegex = Regex("Spec", RegexOptions.Compiled 
                                  ||| RegexOptions.IgnoreCase
                                  ||| RegexOptions.CultureInvariant )

    let descRegex = Regex("Describe", RegexOptions.Compiled 
                                      ||| RegexOptions.IgnoreCase
                                      ||| RegexOptions.CultureInvariant )

    /// Runs all specs in executing assembly.
    let Run() =
        let runTest (spec : Spec) =
            try                                
                spec()
                printfn "OK"
            with
                | AssertFailed x -> printfn "%s" x                
                | x -> printfn "%s\r\n%s" x.Message (x.ToString())                
        

        let asm = System.Reflection.Assembly.GetEntryAssembly()                    
        asm.GetTypes()
        |> Seq.filter(fun t -> (FSharpType.IsModule t) && specRegex.IsMatch(t.Name))
        |> Seq.filter(fun t -> ( printTitle t.Name ; true ) )
        |> Seq.collect(fun t -> t.GetNestedTypes())   
        |> Seq.filter(fun t -> (FSharpType.IsModule t) && descRegex.IsMatch(t.Name))
        |> Seq.filter(fun t -> ( printfn "\r\n\t%s\r\n" t.Name ; true ) )
        |> Seq.collect(fun t -> t.GetMembers())   
        |> Seq.filter(function
                      | :? PropertyInfo as p -> p.PropertyType = typeof<Spec>
                      | :? MethodInfo as m -> m.ReturnType = typeof<Spec>
                      | _ -> false )
        |> Seq.map(fun m -> (m, (match m with
                                 | :? PropertyInfo as p -> Some(p.GetValue(null, null) :?> Spec)
                                 | :? MethodInfo as m -> Some(m.Invoke(null, null) :?> Spec)
                                 | _ -> None)))                    
        |> Seq.filter(fun (m, _) -> ( printf "\t\t- Should %s - " m.Name ; true ) )
        |> Seq.iter(fun (_, s) -> runTest s.Value)