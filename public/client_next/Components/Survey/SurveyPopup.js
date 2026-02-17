import * as React from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import { getSurveyList, userNotFinishedSurveysUri } from '../../api/survey';
import Link from 'next/link';
import { Carousel } from 'react-bootstrap';
import styles from '../../styles/survey.module.css';

export default function SurveyPopup({ user, getLocal }) {
  const [surveys, setSurveys] = React.useState([]);
  const [open, setOpen] = React.useState(false);

  React.useEffect(() => {
    if (user) {
      userNotFinishedSurveysUri().then((data) => {
        setSurveys(data.data.$values);
      });
    }
  }, [user]);
  return user && surveys && surveys.length > 0 ? (
    <>
      <Carousel variant="light" indicators={false}>
        {surveys.map((survey, index) => (
          <Carousel.Item key={index}>
            <Card sx={{ backgroundColor: '#b6e6f4' }}>
              <CardContent>
                <Typography variant="body2" component="div">
                  <Box
                    sx={{
                      display: 'flex',
                      flexDirection: 'column',
                      alignItems: 'center',
                      cursor: 'pointer',
                    }}
                  >
                    <Link
                      href={`/myServices/surveyQuestionForm?surveyId=${survey.id}`}
                      legacyBehavior>
                      <span title={survey.title} className={styles.surveyText}>
                        {survey.title}
                      </span>
                    </Link>
                  </Box>
                </Typography>
              </CardContent>
            </Card>
          </Carousel.Item>
        ))}
      </Carousel>
    </>
  ) : (
    ''
  );
}
