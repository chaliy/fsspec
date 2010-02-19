*FsSpec* is about writing executable specs in F#. Main intent is to provide readable DSL. Hope this tool will be able to achieve this. 

## Syntax

Syntax is inspired by RSpec. Specification of the Calculator could look like this:


    do describe "Calculator" (fun unit ->    

        it "should add two integers" (fun unit -> 

            let res = Calc.Add 2 2
            res.should_be_equal_to 4
        )


        it "should devide two integers" (fun unit -> 

            let res = Calc.Div 2 2
            res.should_be_equal_to 1
        )

    )
	
Another syntax is:

    module ``Calculator Specification``

    open FsSpec
    open Example.Sut

    let ``Describe simple opertions`` =    

        it "should add two integers" (fun unit -> 

            let res = Calc.Add 2 2
            res.should_be_equal_to 4
        )

        it "should devide two integers" (fun unit -> 

            let res = Calc.Div 2 2
            res.should_be_equal_to 1
        )


## Usage

There are two scenarious, first is to write specs as simple app. Thou runnig specs means run app:

    1. Reference FsSpec.dll
    2. Write specification
	
You can find example of this usage in `src/Example` or `src/SuiteExample`.
	
Another option is to use built-in runner

	1. Reference FsSpec.dll
    2. Write specifications
	3. Add Runner.Run() to your main method	
	
Example of this usage located in `src/CompiledSuiteExample`.

## Examples

There are three examples come out of the box. 

First one is located at `\src\Example\`, this is simple compiled spec. To run this spec, compile and run as regular command line app.

Second example on the other hand is implemented as bunch of F# scripts. You can find it at `\src\SuiteExample\`. Because of nature of the script, you do not have to compile it. Just execute Run.cmd. Should work.

And finally third example. You can find it in `src/CompiledSuiteExample`. It describe specification that could be runned with built-in runner.

## References

Introduction post (outdated) - [FsSpec: Introducing yet another Unit Testing/BDD framework for F#]<http://chaliy.name/blog/2009/12/introducing_fsspec>