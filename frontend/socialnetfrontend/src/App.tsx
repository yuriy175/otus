import React, { lazy, Suspense } from 'react'
import AttributesComponent from './components/AttributesComponent'
import { BrowserRouter } from 'react-router-dom'

const App: React.FC = () => {
  return (
    <div >
            <AttributesComponent></AttributesComponent>
    </div>
  )
}

export default App
