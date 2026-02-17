import React from 'react';
import { Grid, Paper } from '@mui/material';
import { CommonBreadcrumb } from '../Components/Layout/CommonBreadcrumb';
import PPDialogCategoryLinks from '../Components/PPDialog/PPDialogCategoryLinks';
import PPDialogIssues from '../Components/PPDialog/PPDialogIssues';
import PPDialogAttachments from '../Components/PPDialog/PPDialogAttachments';
import { useRouter } from 'next/router';
import { getLocalByKey } from './api/menu';
import { getDialog } from '../api/resources';
import { getServiceOwner } from './api/servicetype';
import { Card } from 'react-bootstrap';

function PPDialog({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const [dialog, setDialog] = React.useState(null);
  const [issues, setIssues] = React.useState([]);
  const [documents, setDocuments] = React.useState([]);

  let categoryIdValue = 0;

  if (location.query) {
    const { categoryId } = location.query;
    categoryIdValue = categoryId;
  }
  React.useEffect(() => {
    if (categoryIdValue) {
      getDialog(categoryIdValue).then((data) => {
        setDialog(data.data);
        if (data.data.recommendations && data.data.recommendations.$values) {
          setIssues(data.data.recommendations.$values);
        }
        if (data.data.documents && data.data.documents.$values) {
          setDocuments(data.data.documents.$values);
          console.log(data.data.documents.$values);
        }
      });
    }
  }, [categoryIdValue]);

  const [serviceDescription, setServiceDescription] = React.useState(null);

  React.useEffect(() => {
    getServiceOwner(32).then((data) => {
      setServiceDescription(data.data);
    });
  }, []);

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };

  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };
  const breadcrumbLinks = () => [
    {
      title: getLocal('Home'),
      path: '/',
    },
    {
      title: getLocal('Public Private Dialog'),
      path: '#',
    },
  ];
  
  return (
    <>
      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks()} />
      </div>
      <Grid container spacing={2}>
        <Grid item xs={12} md={12}></Grid>
        <Grid item xs={1} md={1} display={{ xs: "none", md: "block" }}></Grid>
        <Grid item xs={12} sm={7} md={8}>
          <Paper>
            <div className="row">
              <div className="col-sm-12">
                <Card>
                  <Card.Body>
                    {dialog ? (
                      <>
                        <h3>{getLocal(dialog.title)}</h3>
                        {/* // issues */}
                        <PPDialogIssues
                          issues={issues}
                          getLocal={getLocal}
                          getLocalByVal={getLocalByVal}
                        />
                        <PPDialogAttachments
                          attachments={documents}
                          getLocal={getLocal}
                          getLocalByVal={getLocalByVal}
                        />
                      </>
                    ) : serviceDescription ? (
                      <>
                        <strong>{getLocal('Advocacy and Research')}</strong>
                        <br />
                        <pre>
                          {getLocalByVal(
                            serviceDescription.description,
                            serviceDescription.descriptionLocal
                          )}
                        </pre>
                      </>
                    ) : (
                      <h3>{getLocal('No Category Selected')}</h3>
                    )}
                  </Card.Body>
                </Card>
              </div>
            </div>
          </Paper>
        </Grid>
        <Grid item xs={12} sm={2} md={3}>
          <PPDialogCategoryLinks
            getLocal={getLocal}
            getLocalByVal={getLocalByVal}
          />
        </Grid>
      </Grid>
    </>
  );
}

export default PPDialog;
