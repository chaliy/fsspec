module ``Assertions Specification``
open FsSpec

module ``Describe should_be_true and should_be_false assertions`` =
    
    let ``assert TRUE is TRUE`` = spec {        
        true.should_be_true
    }

    let ``assert FASLE is FALSE`` = spec {        
        false.should_be_false
    }

