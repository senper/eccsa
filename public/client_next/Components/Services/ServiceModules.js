import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import Link from 'next/link';
import React from 'react';
import { getServiceOwners } from '../../pages/api/servicetype';
import { getPathFromTitle } from '../../pages/api/menu';

export default function ServiceModules({ getLocal, getLocalByVal }) {
  const [serviceOwners, setServiceOwners] = React.useState([]);
  React.useEffect(() => {
    getServiceOwners().then((data) => {
      setServiceOwners(data.data.$values);
    });
  }, []);

  return (
    <Grid item xs={12} md={12}>
      <div className="bg">{getLocal('Our Services')}</div>
      <div className="image-carousel-container">
        <>
          <hr />
          {serviceOwners
            ? serviceOwners.map((so, index) => (
                <Accordion style={{ marginBottom: '6px' }} key={index}>
                  <AccordionSummary
                    expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}
                  >
                    <div className="icon"></div>
                    <br />
                    <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                      <p className="links">
                        {getLocalByVal(so.title, so.titleLocal)}
                      </p>
                    </div>
                  </AccordionSummary>
                  <AccordionDetails style={{ display: 'block' }}>
                    <pre>
                      {getLocalByVal(so.description, so.descriptionLocal)}
                    </pre>
                  </AccordionDetails>
                  <Link
                    href={`/services/${getPathFromTitle(so.title)}`}
                    className="btn btn-outline-success">

                    {getLocal('Read More')}

                  </Link>
                </Accordion>
              ))
            : null}
        </>
      </div>
    </Grid>
  );
}
