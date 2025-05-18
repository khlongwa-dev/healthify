import { createContext, useEffect, useState } from "react"
import axios from 'axios'
import { toast } from 'react-toastify'

export const AppContext = createContext()

const AppContextProvider = ({ children }) => {
    const backendUrl = import.meta.env.VITE_BACKEND_URL
    const currencySymbol = 'R'

    const [doctors, setDoctors] = useState([])
    const [userToken, setUserToken] = useState(localStorage.getItem('token') || false)
    const [userProfile, setUserProfile] = useState(false)

    // Fetch all doctors from the backend
    const fetchDoctors = async () => {
        try {
            const response = await axios.get(`${backendUrl}api/healthify/doctors-list`)
            const { success, doctors, message } = response.data

            if (success) {
                setDoctors(doctors)
            } else {
                toast.error(message)
            }
        } catch (error) {
            console.error("Error fetching doctors:", error)
            toast.error(error.message)
        }
    }

    // Load user profile if token is present
    const fetchUserProfile = async () => {
        try {
            const response = await axios.get(`${backendUrl}api/user/get-profile`, {
                headers: { Authorization: `Bearer ${userToken}` }
            })

            const { success, user, message } = response.data

            if (success) {
                setUserProfile(user)
                toast.success(message)
            } else {
                toast.error(message)
            }
        } catch (error) {
            console.error("Error fetching user profile:", error)
            toast.error(error.message)
        }
    }

    // Calculates age from a given date of birth
    const calculateAge = (dateOfBirth) => {
        const today = new Date()
        const birthDate = new Date(dateOfBirth)
        const age = today.getFullYear() - birthDate.getFullYear()
        return age
    }

    // Initial fetch of doctor list
    useEffect(() => {
        fetchDoctors()
    }, [])

    // Fetch user profile on token update
    useEffect(() => {
        if (userToken) {
            fetchUserProfile()
        } else {
            setUserProfile(false)
        }
    }, [userToken])

    const contextValue = {
        backendUrl,
        currencySymbol,
        doctors,
        fetchDoctors,
        userProfile,
        setUserProfile,
        userToken,
        setUserToken,
        fetchUserProfile,
        calculateAge
    }

    return (
        <AppContext.Provider value={contextValue}>
            {children}
        </AppContext.Provider>
    )
}

export default AppContextProvider
