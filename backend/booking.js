//api to book appointment

const bookAppointment = async (req, res) => {
    try {
        const {userId, docId, slotDate, slotTime} = req.body
        const docData = await doctorModel.findById(docId)

        if (!docData.available) {
            return res.json({success:false, message:'doctor not found'})
        }

        let slots_booked = docData.slots_booked

        // checking slot availability
        if (slots_booked[slotDate]) {
            if (slots_booked[slotDate].includes(slotTime)) {
                return res.json({success:false, message:'slot not available'})
            } else {
                slots_booked[slotDate].push(slotTime)
            }
        } else {
            slots_booked[slotDate] = []
            slots_booked[slotDate].push(slotTime)
        }

        const userData = await userModel.findById(docId)

        delete docData.slots_booked

        // add booked_slots on a doctor model
        
    } catch (error) {
        
    }
}