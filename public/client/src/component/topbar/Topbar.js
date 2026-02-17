import React, { Component } from 'react';
import {
    BrowserRouter as Router,
    
    Link
    
  } from "react-router-dom";
import './Topbar.css'

class Topbar extends Component {
 
    render() {
        return(
            <ul className="TopbarItems">
           <li><Link to="/am">አማርኛ </Link> | <Link to="/en">English</Link></li>
             </ul>  
            )
    }
}

export default Topbar