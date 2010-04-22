$Env:Path= ($Env:Path + ";C:\Windows\Microsoft.NET\Framework64\v4.0.30128\")

function Pack-FsSpec($name) {

	cd temp

	& ..\lib\zip.exe -9 -A `
		("..\release\" + $name + ".zip") `
		FsSpec.dll `
		FsSpec.pdb `
		FsSpec.xml
			
	cd ..
}

& msbuild "src\FsSpec.sln" /p:TargetFrameworkVersion=4.0 /p:OutputPath="..\..\temp\"

Pack-FsSpec("FsSpec_dotNET4.0")

& msbuild "src\FsSpec.sln" /p:OutputPath="..\..\temp\"

Pack-FsSpec("FsSpec_dotNET3.5")
		
del temp -Force -Recurse