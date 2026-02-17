
import React, { Component } from 'react'
import ReactPlayer from 'react-player'
import './Video.css'

class Video extends Component {
  render () {
    return (
    <div class='player'> <ReactPlayer url='https://youtu.be/Q0IlB1-QH5s?t=199' playing /></div>
    ) }
}

export default Video