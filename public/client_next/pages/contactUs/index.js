import React from 'react';
import { Grid, Paper, Snackbar, Breadcrumbs } from '@mui/material';
import Maps from '../../Components/Map/Map';
import { saveContactUs } from '../api/contactUs';
import { useForm } from 'react-hook-form';
import { AddAlert } from '@mui/icons-material';
import { useRouter } from 'next/router';
import { getLocalByKey } from '../api/menu';
import { CommonBreadcrumb } from '../../Components/Layout/CommonBreadcrumb';

export default function ContactPage({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };

  const { register, handleSubmit, setValue, getValues, formState } = useForm();
  const onSubmit = (inputData) => {
    saveContactUs(inputData)
      .then((data) => {
        if (data.data.saved) {
          Reset();
          setErrorMsg(getLocal('Submitted!!'));
          showNotification('tr');
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification('tr');
        }
      })
      .catch((error) => {
        showNotification('tr');
      });
  };
  const Reset = () => {
    const values = getValues();
    for (const [key] of Object.entries(values)) {
      setValue(key, '');
    }
  };
  ///
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
  const breadcrumbLinks = () => [
    {
      title: getLocal('Home'),
      path: '/',
    },
    {
      title: getLocal('Contact Us'),
      path: '#',
    },
  ];
  return (
    <>
      <Grid container spacing={2} className="grid">
        <div className="paddedMainDiv">
          <CommonBreadcrumb links={breadcrumbLinks()} />
        </div>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={12} md={12}>
          <Paper>
            <div className="container">
              <div className="row">
                <div className="col-md-6">
                  {
                    <form onSubmit={handleSubmit(onSubmit)}>
                      <div className="form-group">
                        <label htmlFor="name">{getLocal('Name')}</label>
                        <input
                          type="text"
                          className="form-control"
                          id="name"
                          placeholder={getLocal('Enter Name')}
                          {...register('name', {
                            required: true,
                            maxLength: 50,
                          })}
                          style={{
                            borderColor: formState.errors['name'] ? 'red' : '',
                          }}
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="email">
                          {getLocal('Email Address')}
                        </label>
                        <div className="input-group">
                          <span className="input-group-addon">
                            <span className="glyphicon glyphicon-envelope"></span>
                          </span>
                          <input
                            type="email"
                            className="form-control"
                            id="email"
                            placeholder={getLocal('Enter Email')}
                            {...register('email', {
                              required: true,
                              maxLength: 50,
                            })}
                            style={{
                              borderColor: formState.errors['email']
                                ? 'red'
                                : '',
                            }}
                          />
                        </div>
                      </div>
                      <div className="form-group">
                        <label htmlFor="name">{getLocal('Message')}</label>
                        <textarea
                          name="message"
                          id="message"
                          className="form-control"
                          rows="9"
                          required="required"
                          placeholder={getLocal('Message')}
                          {...register('message', {
                            required: true,
                          })}
                          style={{
                            borderColor: formState.errors['message']
                              ? 'red'
                              : '',
                          }}
                        ></textarea>
                      </div>
                      <div className="form-group">
                        <button
                          type="submit"
                          className="btn btn-primary pull-right"
                          id="btnContactUs"
                        >
                          {getLocal('Send Message')}
                        </button>
                      </div>
                    </form>
                  }
                </div>
                <div className="col-md-6">
                  <form>
                    <legend>
                      <span className="glyphicon glyphicon-globe"></span>
                      {getLocal('Our Office')}
                    </legend>
                  </form>
                  <Maps />
                </div>
              </div>
            </div>
          </Paper>
        </Grid>

        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={errorMsg ? errorMsg : getLocal('Failed to Save')}
          open={error}
          onClose={() => setError(false)}
        />
      </Grid>
    </>
  );
}
