import { createContext, useEffect, useState } from "react";

var ScreenWidthContext = createContext(window.innerWidth);

const ScreenWidthContextProvider = ({children}: any) => {
  const [screenWidth, setScreenWidth] = useState(window.innerWidth);

  useEffect(() =>
  {
    window.addEventListener("resize", () => setScreenWidth(window.innerWidth))
  }, [])

  return (
    <ScreenWidthContext.Provider value={screenWidth}>
      {children}
    </ScreenWidthContext.Provider>
  )
}

export { ScreenWidthContext, ScreenWidthContextProvider }