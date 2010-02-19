module ``Calculator Add Operation Specification``
open FsSpec

module ``Describe simple add cases`` = 

    let ``add two integers`` = spec {
        let res = Calc.Add 2 2
        res.should_be_equal_to 4 
    }

    let ``add two integers (Unexpected Result)`` = spec {       
        let res = Calc.Add 2 2
        res.should_be_equal_to 5
    }   