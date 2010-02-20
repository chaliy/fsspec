module ``Calculator Add Operation Specification``
open FsSpec

module ``Describe simple add cases`` =     

    let res = Calc.Add 2 2

    let ``add two integers`` = spec {
        res.should_be_equal_to 4 
    }

    let ``add two integers (Unexpected Result)`` = spec {        
        res.should_be_equal_to 5
    }   

module ``Describe advanced add cases`` =         

    let ``add two positive and negative`` = spec {
        let res = Calc.Add 10 -10
        res.should_be_equal_to 0
    }    