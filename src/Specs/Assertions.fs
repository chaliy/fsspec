module ``Assertions Specification``
open FsSpec

module ``Describe Boolean assertions`` =
    
    let ``assert TRUE is TRUE`` = spec {        
        true.should_be_true
    }

    let ``assert FASLE is FALSE`` = spec {        
        false.should_be_false
    }

module ``Describe Collections assertions`` =
    
    let ``assert empty sequence is empty`` = spec {        
        Seq.empty<string>.should_be_empty
    }

    let ``assert non empty sequence is not empty`` = spec {        
        ["test"; "test2"].should_not_be_empty
    }

    let ``assert two item sequence has two items`` = spec {        
        ["test"; "test2"].should_has_items_of 2
    }

