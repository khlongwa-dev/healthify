import React, { useEffect } from 'react'
import { assets } from '../assets/assets';
import { NavLink, useNavigate } from 'react-router-dom';

const Navbar = () => {

  const navigate = useNavigate();
  const [showMenu, setShowMenu] = useState(false);
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const [token, setToken] = useState(true);
  const dropdownRef = useRef(null); // Create a ref for the dropdown

  useEffect(()=> {
    const handleClickOustide = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setDropdownOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOustide);

    return () => {
      document.removeEventListener('mousedown', handleClickOustide);
    };
  }, []);

  return (
    <div>
      <img onClick={() => navigate('/')} src={assets.logo} alt="" />
      <ul>
        <NavLink to='/'>
          <li>HOME</li>
          <hr />
        </NavLink>
        <NavLink to='/doctors'>
          <li>ALL DOCTORS</li>
          <hr />
        </NavLink>
        <NavLink to='/about'>
          <li>ABOUT</li>
          <hr />
        </NavLink>
        <NavLink to='/contacts'>
          <li>CONTACTS</li>
          <hr />
        </NavLink>
      </ul>
      
      <div>
        {
          token
          ? <div>
            <img src={assets.profile_pic} alt="" />
            <img src={assets.dropdown_icon} alt="" />
            {dropdownOpen && (
              <div ref={dropdownRef}>
                <div>
                  <p onClick={() => { navigate('/my-profile'); setDropdownOpen(false); }}></p>
                  <p onClick={() => { navigate('/my-appointments'); setDropdownOpen(false); }}></p>
                  <p onClick={() => { setToken(false); setDropdownOpen(false); }}></p>
                </div>
              </div>
            )}
          </div>
          : <button onClick={() => navigate('/login')} >Create Account</button>
        }

        <img onClick={() => setShowMenu(true)} src={assets.menu_icon} alt="" />
      </div>
    </div>
  )
}

export default Navbar
