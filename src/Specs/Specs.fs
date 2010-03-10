module ``Specs Specification``

open FsSpec

module ``Describe should_be_true and should_be_false assertions`` =

    let disposable() = { new System.IDisposable with
                      member x.Dispose() = () }
    
    let ``support use conxtruction in spec`` = spec {
        justfail()
    }    
