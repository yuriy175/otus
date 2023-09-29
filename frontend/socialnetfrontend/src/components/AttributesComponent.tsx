import React, { useEffect, useState } from 'react';
import axios from 'axios'

function AttributesComponent() {
  
  const [appState, setAppState] = useState();
  
  useEffect( async() => {
    try{
      const apiUrl = 'http://localhost:5007/api/Attributes';
      const attributes = await axios.get(apiUrl);
      const y = 0;
    }
    catch(err){
      console.log(err);
    }
  }, [setAppState]);

 
  return (
    <div className="app">
      Уряяя!!!!
    </div>
  );
}

export default AttributesComponent;