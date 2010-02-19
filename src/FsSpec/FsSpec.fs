namespace FsSpec

exception AssertFailed of string

module Runner =
    open System.Reflection
    //open Microsoft.FSharp.Reflection
    open Microsoft.FSharp.Metadata

    let internal printTitle (title : string) = 
            let sep = (new System.String('-', title.Length))
            printfn "\r\n%s\r\n%s\r\n%s\r\n"  sep title sep 

//    let Run() =
//
//        let asm = System.Reflection.Assembly.GetEntryAssembly()                    
//        let fasm = FSharpAssembly.FromAssembly(asm)
//        fasm.Entities
//            |> Seq.filter(fun e -> e.IsModule)
//            |> Seq.filter(fun e -> e.LogicalName.Contains("Spec"))                    
//            |> Seq.filter(fun e -> ( printTitle e.CompiledName ; true ) )
//            |> Seq.collect(fun e -> e.MembersOrValues)
//            |> Seq.filter(fun m -> m.LogicalName.Contains("Describe"))
//            |> Seq.filter(fun m -> ( printfn "\t%s\r\n" m.CompiledName ; true ) )
//            |> Seq.iter(fun m -> (match m.ReflectionMemberInfo with
//                                    | :? PropertyInfo as p -> ignore(p.GetValue(null, null))
//                                    | :? MethodInfo as m -> ignore(m.Invoke(null, null))
//                                 ))
//
//    let Run2() =
//       
//        let runTest (mem : FSharpMemberOrVal) =
//            try                
//                let res = match mem.ReflectionMemberInfo with
//                           | :? PropertyInfo as p -> p.GetValue(null, null)
//                           | :? MethodInfo as m -> m.Invoke(null, null) 
//                printfn "OK"
//            with
//                | AssertFailed x -> printfn "%s" x
//                | :? TargetInvocationException as x -> match x.InnerException with
//                                                         | :? AssertFailed as x -> printfn "%s" x.Message
//                                                         | x -> printfn "%s\r\n%s" x.Message (x.ToString())
//                | x -> printfn "%s\r\n%s" x.Message (x.ToString())                
//        
//        let asm = System.Reflection.Assembly.GetEntryAssembly()
//        let fasm = FSharpAssembly.FromAssembly(asm)        
//        fasm.Entities
//            |> Seq.filter(fun e -> e.IsModule)
//            |> Seq.filter(fun e -> e.LogicalName.Contains("Spec"))
//            |> Seq.filter(fun e -> ( printTitle e.CompiledName ; true ) )
//            |> Seq.collect(fun e -> e.NestedEntities)
//            |> Seq.filter(fun e -> e.IsModule)
//            |> Seq.filter(fun e -> e.LogicalName.Contains("Describe"))
//            |> Seq.filter(fun e -> ( printfn "\t%s\r\n" e.CompiledName ; true ) )
//            |> Seq.collect(fun e -> e.MembersOrValues)            
//            |> Seq.filter(fun m -> m.DisplayName.StartsWith("("))
//            |> Seq.filter(fun m -> ( printf "\t\t-Should %s - " m.CompiledName ; true ) )
//            |> Seq.iter(runTest)
        
    let Run3() =
        let runTest (mem : FSharpMemberOrVal) =
            try                
                let res = (mem.ReflectionMemberInfo :?> PropertyInfo)
                            .GetValue(null, null) :?> (unit -> unit)
                res()
                printfn "OK"
            with
                | AssertFailed x -> printfn "%s" x                
                | x -> printfn "%s\r\n%s" x.Message (x.ToString())                
        

        let asm = System.Reflection.Assembly.GetEntryAssembly()                    
        let fasm = FSharpAssembly.FromAssembly(asm)
        let ee = fasm.Entities
                    |> Seq.filter(fun e -> e.IsModule)
                    |> Seq.filter(fun e -> e.LogicalName.Contains("Spec"))                    
                    |> Seq.filter(fun e -> ( printTitle e.CompiledName ; true ) )
                    |> Seq.collect(fun e -> e.NestedEntities)
                    |> Seq.filter(fun e -> e.IsModule)
                    |> Seq.filter(fun e -> e.LogicalName.Contains("Describe"))
                    |> Seq.filter(fun e -> ( printfn "\t%s\r\n" e.CompiledName ; true ) )
                    |> Seq.collect(fun e -> e.MembersOrValues)
                    |> Seq.filter(fun m -> ( printf "\t\t-Should %s - " m.CompiledName ; true ) )
                    |> Seq.iter(runTest)
                    //|> Seq.filter(fun m -> m.Type = (unit -> unit))
                    //|> Seq.toArray                

        ()


[<AutoOpen>]
module SpecHelpers =
           
    let describe desc (f: unit -> unit) =
        printfn "%s" desc
        f()  
        printfn "\r\n"
            
    let it desc (f: unit -> unit) =        
        try
           do f()
           printfn "\t\t- %s - OK" desc
        with
            | AssertFailed x -> printfn "\t\t- %s - %s" desc x
            | x -> printfn "\t\t- %s - %s\r\n%s" desc x.Message (x.ToString())               

    // Builder

    type Spec = (unit -> unit)

//    type Spec =
//        abstract member Execution : unit -> unit    

//    let succeed x = (fun () -> Some(x)) 
//    let fail      = (fun () -> None) 
    let runSpec (a:Spec) = a() 
//    let bind p rest = (rest (runSpec p))
    let delay f = (fun() -> runSpec (f()))

     
    type SpecBuilder() =
        
//        member b.Return(x) = succeed x
//        member b.Bind(p,rest) = bind p rest
        member b.Delay(f) = delay f
        member b.Zero() = (fun uint -> ())
//        member b.Let(p,rest) : Spec = rest p 

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


//           let throw_exception<'a when 'a :> exn> actual =
//  Assert.Throws<'a>(Assert.ThrowsDelegate(actual))
//
//let throws_exception() : unit =
//  raise(System.ArgumentException "Bad things")
// http://weblogs.asp.net/podwysocki/archive/2008/06/04/language-oriented-programming-and-functional-unit-testing-in-f.aspx
