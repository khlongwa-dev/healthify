import { createContext } from "react";
import { doctors } from "../assets/assets";

export const AppContext = createContext()

const AppContextProvider = (props) => {
    const currencySymbol = 'R';

    const value = {
        doctors,
        currencySymbol
    }

    return (
        <AppContextProvider value={value}>
            {props.value}
        </AppContextProvider>
    )
}

export default AppContextProvider