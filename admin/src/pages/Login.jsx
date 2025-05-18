import React, { useState, useContext } from 'react';
import { AdminContext } from '../context/AdminContext';
import { DoctorContext } from '../context/DoctorContext';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { toast } from 'react-toastify';

const Login = () => {
  const [role, setRole] = useState('Admin');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const { setAdminToken, backendUrl } = useContext(AdminContext);
  const { setDoctorToken } = useContext(DoctorContext);

  const navigate = useNavigate();

  const handleLogin = async (endpoint, tokenKey, setToken) => {
    try {
      const { data } = await axios.post(`${backendUrl}api/authentication/${endpoint}/login`, {
        email,
        password,
      });

      if (data.token) {
        localStorage.setItem(tokenKey, data.token);
        setToken(data.token);
        toast.success(data.message);
      } else {
        toast.error(data.message);
      }
    } catch (error) {
      console.error(error);
      toast.error(error?.response?.data?.message || 'Login failed');
    }
  };

  const onSubmitHandler = async (event) => {
    event.preventDefault();

    if (role === 'Admin') {
      await handleLogin('admin', 'adminToken', setAdminToken);
    } else {
      await handleLogin('doctor', 'doctorToken', setDoctorToken);
    }
  };


  return (
    <form onSubmit={onSubmitHandler} className='min-h-[80vh] flex items-center'>
        <div className='flex flex-col gap-3 m-auto items-start p-8 min-w-[340px] sm:min-w-96 border rounded-xl text-[#5E5E5E] text-sm shadow-lg'>
            <p className='text-2xl font-semibold m-auto'><span className='text-primary'> {role} </span> Login</p>
            <div className = 'w-full'>
                <p>Email</p>
                <input onChange={(e)=> setEmail(e.target.value)} value={email} className='border border-[#DADADA] rounded w-full p-2 mt-1' type="email" required />
            </div>
            <div className = 'w-full'>
                <p>Password</p>
                <input onChange={(e)=> setPassword(e.target.value)} value={password} className='border border-[#DADADA] rounded w-full p-2 mt-1' type="password" required />
            </div>
            <button className='bg-primary text-white w-full py-2 rounded-md text-base' type='submit'>Login</button>
            {
                role === 'Admin'
                ? <p>Doctor Login? <span className='text-primary underline cursor-pointer' onClick={()=>setRole('Doctor')}>Click here</span></p>
                : <p>Admin Login? <span className='text-primary underline cursor-pointer' onClick={()=>setRole('Admin')}>Click here</span></p>
            }
        </div>
    </form>
  )
}

export default Login
