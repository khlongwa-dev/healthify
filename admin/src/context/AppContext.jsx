import React, { createContext } from 'react'

export const AppContext = createContext()

const AppContextProvider = ({ children }) => {
  const currencySymbol = 'R'

  const calculateAge = (dob) => {
    const today = new Date()
    const birthDate = new Date(dob)
    const age = today.getFullYear() - birthDate.getFullYear()
    return age
  }

  const monthNames = ['', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']

  const formatSlotDate = (slotDate) => {
    const [day, month, year] = slotDate.split('-')
    return `${day} ${monthNames[Number(month)]} ${year}`
  }

  const contextValue = {
    calculateAge,
    formatSlotDate,
    currencySymbol
  }

  return (
    <AppContext.Provider value={contextValue}>
      {children}
    </AppContext.Provider>
  )
}

export default AppContextProvider
