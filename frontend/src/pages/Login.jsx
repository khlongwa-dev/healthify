import React, { useState } from 'react'

const Login = () => {
  const [state, setState] = useState('Sign Up')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [name, setName] = useState('')

  const onSubmitHandler = async (event)=> {
    event.preventDefault()
  }
  
  return (
    <form>
      <div>
        <p>{state === 'Sign Up' ? "Create Account" : "Login"}</p>
        <p>Please {state === 'Sign Up' ? "sign up" : "login in"} to book appointment.</p>
        
        {
          state === "Sign Up" &&
          <div>
            <p>Full Name</p>
            <input type="text" onChange={(e)=>setName(e.target.name)} value={name}/>
          </div>
        }

        <div>
          <p>Email</p>
          <input type="email" onChange={(e)=>setEmail(e.target.email)} value={email}/>
        </div>

        <div>
          <p>Password</p>
          <input type="password" onChange={(e)=>setPassword(e.target.password)} value={password}/>
        </div>

        <button>{state === 'Sign Up' ? "Create Account" : "Login"}</button>
        {
          state === 'Sign Up'
          ? <p>Already have an account? <span onClick={()=>setState('Login')} className='text-primary underline cursor-pointer'>Login here</span></p>
          : <p>Create a new account? <span onClick={()=>setState('Sign Up')} className='text-primary underline cursor-pointer'>Click here</span></p>
        }
      </div>
    </form>
  )
}

export default Login
