do FsSpec.Runner.Run()

//open System.Reflection
//open Microsoft.FSharp.Reflection
//
//let asm = Assembly.GetEntryAssembly()
//asm.GetTypes()
//        |> Seq.filter(fun x -> FSharpType.IsModule x)
//        |> Seq.map(fun x ->
//                            printfn "First level modules %s" x.FullName 
//                            x )
//        |> Seq.collect(fun x -> x.GetNestedTypes())   
//        |> Seq.iter(fun x -> printfn "Second level modules %s" x.FullName)