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


    let should_fail (s : unit -> unit) =
        try
           do s()
           raise (AssertFailed(sprintf "Nothing failed!"))
        with | _ -> () 


//           let throw_exception<'a when 'a :> exn> actual =
//  Assert.Throws<'a>(Assert.ThrowsDelegate(actual))
//
//let throws_exception() : unit =
//  raise(System.ArgumentException "Bad things")
// http://weblogs.asp.net/podwysocki/archive/2008/06/04/language-oriented-programming-and-functional-unit-testing-in-f.aspx

/// All about running specs
module Runner =
    open System.Reflection    
    open Microsoft.FSharp.Metadata

    let internal printTitle (title : string) = 
            let sep = (new System.String('-', title.Length + 4))
            printfn "\r\n%s\r\n  %s  \r\n%s\r\n"  sep title sep 

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
        let fasm = FSharpAssembly.FromAssembly(asm)
        fasm.Entities
                    |> Seq.filter(fun e -> e.IsModule)
                    |> Seq.filter(fun e -> e.LogicalName.Contains("Spec"))                    
                    |> Seq.filter(fun e -> ( printTitle e.CompiledName ; true ) )
                    |> Seq.collect(fun e -> e.NestedEntities)
                    |> Seq.filter(fun e -> e.IsModule)
                    |> Seq.filter(fun e -> e.LogicalName.Contains("Describe"))
                    |> Seq.filter(fun e -> ( printfn "\r\n\t%s\r\n" e.CompiledName ; true ) )
                    |> Seq.collect(fun e -> e.MembersOrValues) 
                    |> Seq.map(fun m -> (m, (match m.ReflectionMemberInfo with
                                               | :? PropertyInfo as p -> Some(p.GetValue(null, null))
                                               | :? MethodInfo as m -> Some(m.Invoke(null, null))
                                               | _ -> None)))                    
                    |> Seq.filter(fun (_ , s) -> s.IsSome) 
                    |> Seq.map(fun (m, s) -> (m, (match s.Value with
                                                    | :? Spec as spec -> Some(spec)
                                                    | _ -> None )))
                    |> Seq.filter(fun (_ , s) -> s.IsSome) 
                    |> Seq.filter(fun (m, _) -> ( printf "\t\t- Should %s - " m.CompiledName ; true ) )
                    |> Seq.iter(fun (_, s) -> runTest s.Value)         