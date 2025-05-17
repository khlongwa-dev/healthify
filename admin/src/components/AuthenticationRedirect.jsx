import { useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { AdminContext } from '../context/AdminContext';
import { DoctorContext } from '../context/DoctorContext';

const AuthRedirect = () => {
  const { adminToken } = useContext(AdminContext);
  const { doctorToken } = useContext(DoctorContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (adminToken) {
      navigate('/admin-dashboard');
    } else if (doctorToken) {
      navigate('/doctor-dashboard');
    }
  }, [adminToken, doctorToken, navigate]);

  return null;
};

export default AuthRedirect;
