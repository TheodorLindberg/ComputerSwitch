import React, { useContext, useEffect, useState } from "react"


export interface Code
{
    code: string | null;
    setCode: (code: string) => void,
    showDialog: boolean,
    setShowDialog: (show: boolean) => void
}

const CodeContext = React.createContext<Code>({code:null, setCode: () => undefined, showDialog: false, setShowDialog: (show: boolean) => undefined })

export function useCodeContext() {
    return useContext(CodeContext)
}

function CodeProvider({children}:any) {
    const [code, setCode] = useState<string | null>("test");
    const [showDialog, setShowDialog] = useState<boolean>(true);
    
    const updateCode = function(code: string) {
        localStorage.setItem("code", code)
        document.cookie = ""
        setShowDialog(false)
        setCode(code)
    }

    useEffect(() => {
        if(localStorage.getItem("code")) {
            updateCode(localStorage.getItem("code") as string)
            setShowDialog(false);
        }
    }, []);
    
    return <CodeContext.Provider value={{code:code, setCode: updateCode, showDialog: showDialog, setShowDialog: (show) => setShowDialog(show)}}>{children}</CodeContext.Provider>;
}

export default CodeProvider;
