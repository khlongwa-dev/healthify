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
      
    </div>
  )
}

export default Navbar
