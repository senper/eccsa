import React, { useState } from 'react';
import { Grid, Snackbar } from '@mui/material';
import { Row, Col, Form } from 'react-bootstrap';

import { useForm } from 'react-hook-form';
import { AddAlert } from '@mui/icons-material';
import { changePassword } from '../api/oauth';


export default function ChangePassword() {
  window.scrollTo({
    top: 0,
  });
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm();
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
  const showNotification = (place) => {
    switch (place) {
      case 'tr':
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg(null);
          }, 6000);
        }
        break;
      default:
        break;
    }
  };

  function resetInputs(inputData) {
    for (const [key, value] of Object.entries(inputData)) {
      setValue(key, '');
    }
  }

  function onSubmit(inputData) {
    if (inputData.confirmPassword !== inputData.newPassword) {
      setErrorMsg('Password & Confirm Password are not similar!');
      showNotification('tr');
      return;
    }
    changePassword(inputData)
      .then((data) => {
        if (data.data.success) {
          setErrorMsg('Password changed successfully!');
          resetInputs(inputData);
          //history.push("/login");
        } else if (data.data.errorMsg) {
          setErrorMsg(data.data.errorMsg);
        }
        showNotification('tr');
      })
      .catch((error) => {
        console.log(error);
        //setErrorMsg(error);
        showNotification('tr');
      });
  }
  return (
    <div>
      <Grid container spacing={2} className="grid">
        <Grid item xs={3} md={3}></Grid>
        <Grid item xs={6} md={6}>
          <div className="row">
            <div className="bg">Change Password </div>
          </div>
          <Form onSubmit={handleSubmit(onSubmit)}>
            <Row className="g-2">
              <Col md>
                <Form.Label> Old Password</Form.Label>

                <Form.Control
                  type="password"
                  {...register('oldPassword', {
                    required: true,
                    maxLength: 200,
                  })}
                />
              </Col>
              <Col md></Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> New Password</Form.Label>

                <Form.Control
                  type="password"
                  {...register('newPassword', {
                    required: true,
                    maxLength: 200,
                  })}
                />
              </Col>
              <Col md>
                <Form.Label> Confirm Password</Form.Label>

                <Form.Control
                  type="password"
                  {...register('confirmPassword', {
                    required: true,
                    maxLength: 200,
                  })}
                />
              </Col>
            </Row>
            <button type="submit" className="btn btn-success">
              Save
            </button>
            <br />
          </Form>
        </Grid>
        <Grid item xs={3} md={3}></Grid>

        <br />
        <br />
      </Grid>
      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={errorMsg ? errorMsg : 'Failed to Update'}
        open={error}
        onClose={() => setError(false)}
        
      />
    </div>
  );
}

// export default App;
