import React from 'react'
import Header from '../components/Header'
import SpecialtyMenu from '../components/SpecialtyMenu'
import TopDoctors from '../components/TopDoctors'
import Banner from '../components/Banner'

const Home = () => {
  return (
    <div>
      <Header/>
      <SpecialtyMenu/>
      <TopDoctors/>
      <Banner/>
    </div>
  )
}

export default Home
